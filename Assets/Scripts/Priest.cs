using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code taken from https://www.youtube.com/watch?v=_QajrabyTJc
// and https://www.youtube.com/watch?v=ixM2W2tPn6c
public class Priest : MonoBehaviour
{
    private Rigidbody body;
    public Camera cam;

    private const float movementSpeed = 5f;
    private const float rotationSpeed = 200f;

    private float curXRotation;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // taken from https://www.youtube.com/watch?v=EANtTI6BCxk
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit rayHit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out rayHit, 10))
            {
                if (rayHit.transform)
                {
                    print(rayHit.transform.gameObject.name);
                }
            }
        }
    }

    // make for smooth movements due to the fixed interval
    void FixedUpdate()
    {
        // keyboard movement
        float x = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        Vector3 translate = transform.right * x + transform.forward * z;
        body.MovePosition(transform.position + translate);

        // mouse movement
        float xMouse = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float yMouse = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // look sideway
        transform.Rotate(Vector3.up * xMouse);

        // look up down
        curXRotation -= yMouse;
        curXRotation = Mathf.Clamp(curXRotation, -90, 90);
        cam.transform.localRotation = Quaternion.Euler(curXRotation, 0f, 0f);
    }
}
