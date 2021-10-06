using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float walkSpeed = 10.0f;
	public float turnSpeed = 400.0f;
	public float gravity = 20.0f;
	public float airControl = 10;
	public float jumpHeight = 3;
	public float wallRideSpeedMultiplier = 1.5f;
	public float springJump = 30;
	public float dashSpeed = 30;
	public float dashCooldown = 2;
	public AudioClip jumpSFX;
	public AudioClip wallrunSFX;
	public DeathReset deathReset;

	Rigidbody rb;
	float timeStamp;
	AudioSource audioSource;
	int jumpCount = 2;
	Animator anim;
	CharacterController controller;
	Vector3 input, moveDirection;
	bool onWall = false;
	bool isDashing = false;

	void Start () {
		controller = GetComponent <CharacterController>();
		anim = gameObject.GetComponentInChildren<Animator>();
		audioSource = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody>();
	
	}

	void Update(){

		float moveHorizontal = 0;
		float moveVertical = 0;

		// Input.GetAxis emulates a joystick and makes the controls sloppy
		if (Input.GetKey("w")) {
			moveVertical += 1;
		}

		if (Input.GetKey("s")) {
			moveVertical -= 1;
		}

		if (Input.GetKey("a")) {
			moveHorizontal -= 1;
		}

		if (Input.GetKey("d")) {
			moveHorizontal += 1;
		}


		if (moveHorizontal == 0 && moveVertical == 0) {
			anim.SetInteger("AnimationPar", 0);
		} else {
			anim.SetInteger("AnimationPar", 1);
        }

		input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

		if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("s"))
		{
			input *= walkSpeed;
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
		{
			isDashing = true;
			StartCoroutine(TimerRoutine());
		}

		if (controller.isGrounded)
		{
			jumpCount = 2;

			moveDirection = input;


			if (Input.GetButtonDown("Jump"))
			{
				jumpCount--;
				moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
				AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
			}
			else
			{
				moveDirection.y = 0.0f;
			}

		}
		else if(onWall && (moveHorizontal != 0 || moveVertical != 0))
        {
			moveDirection = input * wallRideSpeedMultiplier;
			if (!audioSource.isPlaying)
            {
				audioSource.clip = wallrunSFX;
				audioSource.Play();
            }
		}
		else
		{
			if (Input.GetButtonDown("Jump") && jumpCount > 0)
			{
				moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity / 2);
				AudioSource.PlayClipAtPoint(jumpSFX, transform.position);
				jumpCount--;
			}
			input.y = moveDirection.y;
			moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
		}

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);

		if (input.magnitude > .01f)
		{
			float cameraYawRotation = Camera.main.transform.eulerAngles.y;
			Quaternion newRotation = Quaternion.Euler(0f, cameraYawRotation, 0f);
			transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 10);
		}
	}

    private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("WallRide")) {
			onWall = true;
			jumpCount = 1;
		} else if (other.CompareTag("WallJump")) {
			jumpCount = 1;
        } else if (other.CompareTag("Projectile")) {
			Application.LoadLevel(Application.loadedLevel);
			//deathReset.ResetCheckpoint();
        }

		if (other.gameObject.CompareTag("Spring"))
		{
			moveDirection.y = Mathf.Sqrt(2 * springJump * gravity / 2);
			input.y = moveDirection.y;
			moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
		}
	}

	private void OnTriggerExit(Collider other) {
		if(other.CompareTag("WallRide")) {
			onWall = false;
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spring"))
        {
			moveDirection.y = Mathf.Sqrt(2 * springJump * gravity / 2);
			input.y = moveDirection.y;
			moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
		}
    }

	private IEnumerator TimerRoutine()
    {
		Debug.Log("Dashing");
		input *= dashSpeed;
		yield return new WaitForSeconds(dashCooldown);
		isDashing = false;
	}
}
