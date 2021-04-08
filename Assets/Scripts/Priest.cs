using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code taken from https://www.youtube.com/watch?v=_QajrabyTJc
// and https://www.youtube.com/watch?v=ixM2W2tPn6c
public class Priest : MonoBehaviour
{
    private Rigidbody body;
    public Camera cam;
    public float rayHitDistance;
    public Canvas pointCursor;

    private const float movementSpeed = 5f;
    private const float rotationSpeed = 200f;

    private float curXRotation;

    private RaycastHit rayHit;
    private Ray ray;

    [SerializeField]
    private int investigate;
    private int hitLive;
    private int delayClearTimer;

    private GameObject spriteOFF;
    private GameObject spriteON;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        if (rayHitDistance < 5.0f)
        {
            rayHitDistance = 5.0f;
        }
        investigate = 0;
        hitLive = 0;
        delayClearTimer = 0;

        spriteOFF = pointCursor.GetComponent<Transform>().Find("Crosshair").gameObject;
        spriteON = pointCursor.GetComponent<Transform>().Find("Eye").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        int lastHitLive = hitLive;

        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out rayHit, rayHitDistance))
        {
            if (rayHit.transform)
            {
                switch (rayHit.transform.gameObject.name)
                    {
                    case "WoodenChest_Hit_1":
                        hitLive = 1;
                        break;

                    case "Bible_Hit_2":
                        hitLive = 2;
                        break;

                    case "Monk_Robes_Hit_3":
                        hitLive = 3;
                        break;

                    default:
                        hitLive = 0;
                        break;
                }

                if (lastHitLive != hitLive)
                {
                    if (pointCursor)
                    {
                        if (hitLive == 0)
                        {
                            spriteON.SetActive(false);
                            spriteOFF.SetActive(true);
                        }
                        else
                        {
                            spriteON.SetActive(true);
                            spriteOFF.SetActive(false);
                        }
                    }
                    print(rayHit.transform.gameObject.name + " -- " + hitLive);
                }

                if (hitLive != 0 && Input.GetMouseButtonDown(0))
                {
                    investigate = hitLive;
                    delayClearTimer = 0;
                }
                else
                {
                    // get rid of this after debugging and replace the investigate with proper message dispatch method to the handler!
                    if (delayClearTimer >= 200)
                    {
                        delayClearTimer = 0;
                        investigate = 0;
                    }
                    else
                    {
                        delayClearTimer += 1;
                    }
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
