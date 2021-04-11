using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPopup : MonoBehaviour
{
    private bool isInvestigating;

    public bool IsInvestigating
    {
        get { return isInvestigating; }
    }

    public void Open()
    {
        isInvestigating = true;
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Close()
    {
        isInvestigating = false;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
