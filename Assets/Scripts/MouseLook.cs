using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    Transform cameraTransform;
    Transform playerTransform;
    public Transform targetTransform;

    public float mouseSens = 1;

    private float pitch = 0;

    Vector2 rotation = Vector2.zero;

    Vector3 originalLocal;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = transform.parent.transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotation.y = playerTransform.eulerAngles.y;

        originalLocal = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotation.y += Input.GetAxis("Mouse X") * mouseSens;
        rotation.x -= Input.GetAxis("Mouse Y") * mouseSens;
        rotation.x = Mathf.Clamp(rotation.x, -24, 60);

        playerTransform.eulerAngles = new Vector2(0, rotation.y);
        cameraTransform.eulerAngles = new Vector2(rotation.x, rotation.y);


        Vector3 direction = targetTransform.TransformDirection(originalLocal.normalized);
        float length = originalLocal.magnitude * 8;
        Vector3 origin = cameraTransform.position + direction * 2;

        float scale = 10;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, length)) {

            //Debug.DrawRay(origin, direction * length, Color.yellow);
            //Debug.Log("Did Hit");

            transform.localPosition = targetTransform.InverseTransformPoint(hit.point);

        } else {
            //Debug.DrawRay(origin, direction * length, Color.white);
            //Debug.Log("Did not Hit");

            transform.localPosition = originalLocal;
        }
    }
}