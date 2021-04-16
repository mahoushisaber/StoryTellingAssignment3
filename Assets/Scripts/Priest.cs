using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code taken from https://www.youtube.com/watch?v=_QajrabyTJc
// and https://www.youtube.com/watch?v=ixM2W2tPn6c
public class Priest : MonoBehaviour
{
    // properties
    private Rigidbody body;
    public Camera cam;
    public Canvas canvas;

    // movements
    private const float movementSpeed = 5f;
    private const float rotationSpeed = 250f;
    private float curXRotation;

    // for interacting with scene
    private RaycastHit rayHit;
    private Ray ray;
    public float rayHitDistance=3;
    private GameObject[] interactables;
    private GameObject targetObj;

    // mouse cursors
    private GameObject crossHairIcon;
    private GameObject eyeIcon;

    // story stuff
    private StoryPopup storyPopup;
    public GameObject cutscene;

    // story stage
    private int stage;
    private const int LAST_INTERACTABLE_STAGE = 4;
    private const int ENDING_STAGE = 5;
    private GameObject endingObject;

    void Start()
    {
        body = GetComponent<Rigidbody>();

        // center the scene around mouse
        // will also make default mouse cursor disappear
        Cursor.lockState = CursorLockMode.Locked;
        if (rayHitDistance < 5.0f)
        {
            rayHitDistance = 5.0f;
        }

        targetObj = null;
        crossHairIcon = canvas.GetComponent<Transform>().Find("Crosshair").gameObject;
        eyeIcon = canvas.GetComponent<Transform>().Find("Eye").gameObject;

        storyPopup = canvas.transform.Find("Popup").GetComponent<StoryPopup>();
        stage = 0;
        UpdateInteractables();
        endingObject = GameObject.Find("Endings");
    }

    /// <summary>
    /// Update the interactables in the scene based on the
    /// current story stage.
    /// </summary>
    void UpdateInteractables()
    {
        string stageTag = "Stage" + stage;
        GameObject[] stageInteractables = GameObject.FindGameObjectsWithTag(stageTag);
        foreach (GameObject obj in stageInteractables)
        {
            obj.tag = "Interactables";
        }
        // update the tag and still keep the old interactables
        interactables = GameObject.FindGameObjectsWithTag("Interactables");
    }

    // Update is called once per frame
    void Update()
    {
        if (!storyPopup.IsInvestigating)
        {
            GetMouseOverInteractable();
            if (Input.GetMouseButtonDown(0) && targetObj)
            {
                Investigate(targetObj);
            }
        }
    }

    /// <summary>
    /// Get the interactable object that the user
    /// is mousing over.
    /// </summary>
    /// <returns>The Interactable object if the user is hovering
    /// over it. Else returns null.</returns>
    private void GetMouseOverInteractable()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        // if hits anything that has a transform
        if (Physics.Raycast(ray, out rayHit, rayHitDistance)
            && rayHit.transform)
        {
            // check if we still on the same obj
            // save time from looping
            if (rayHit.transform.gameObject == targetObj)
            {
                return;
            }

            // if hits an interactable
            foreach (GameObject obj in interactables)
            {
                if (rayHit.transform.gameObject == obj)
                {
                    targetObj = obj;
                    ToggleInvestigateCursor(true);
                    return;
                }
            }
            targetObj = null;
            ToggleInvestigateCursor(false);
        }
        targetObj = null;
        ToggleInvestigateCursor(false);
    }

    /// <summary>
    /// Toggle whether the user is in investigate (eye) mode
    /// or not.
    /// </summary>
    /// <param name="isInvestigating"></param>
    private void ToggleInvestigateCursor(bool isInvestigating)
    {
        // if no change in state, do nothing
        if (isInvestigating == eyeIcon.activeSelf) return;

        eyeIcon.SetActive(isInvestigating);
        crossHairIcon.SetActive(!isInvestigating);
    }

    /// <summary>
    /// Investigate the obj passed in and display a menu
    /// for the user to see.
    /// </summary>
    /// <param name="obj"></param>
    private void Investigate(GameObject obj)
    {
        storyPopup.Open(obj);
    }

    public void IncrementStage()
    {
        stage++;
        if (stage == LAST_INTERACTABLE_STAGE)
        {
            cutscene.SetActive(true);
        }
        else if (stage == ENDING_STAGE)
        {
            Investigate(endingObject);
        }

        if (stage <= LAST_INTERACTABLE_STAGE)
        {
            UpdateInteractables();
        }
    }

    // make for smooth movements due to the fixed interval
    void FixedUpdate()
    {
        if (!storyPopup.IsInvestigating)
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
}
