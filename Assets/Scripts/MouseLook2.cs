using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook2 : MonoBehaviour
{
     Transform playerBody;

    public float mouseSensitivity = 10f;

    float pitch = 0;

    void Start()
    {
        playerBody = transform.parent.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //yaw
        playerBody.Rotate(Vector3.up * moveX);

        //pitch
        pitch -= moveY;

        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
