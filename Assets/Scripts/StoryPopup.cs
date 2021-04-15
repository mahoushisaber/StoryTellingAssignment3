using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryPopup : MonoBehaviour
{
    // UI
    public GameObject interactableArea;

    public bool IsInvestigating
    {
        get; private set;
    }

    private Interactable currInteractable;

    /// <summary>
    /// Open the StoryPopup
    /// </summary>
    /// <param name="obj"></param>
    public void Open(GameObject obj)
    {
        IsInvestigating = true;

        // load the object into each premade UI
        Interactable interactable = Resources.Load<Interactable>("Interactables/" + obj.name);
        currInteractable = interactable;

        for (int i = 0; i < interactable.items.Count; i++)
        {
            GameObject itemArea = interactableArea.transform.GetChild(i).gameObject;
            Image img = itemArea.GetComponentInChildren<Image>();
            img.sprite = interactable.items[i].image;
            Text name = itemArea.GetComponentInChildren<Text>();
            name.text = interactable.items[i].itemName;
            itemArea.SetActive(true);
        }

        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Close()
    {
        // rehide everything
        foreach (Transform child in interactableArea.transform)
        {
            child.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
        IsInvestigating = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnInteractableSelected(int InteractableID)
    {
        StoryPresentation StoryPresentaionScript = GameObject.FindObjectOfType<StoryPresentation>();

        // A little fuggly here as the InteractableID inverst because of the Grid Layout Group
        // attached to the Interactable area. It turns out that as you add items it inverts
        // the Interactable ID. Its okay now that it is unsderstood we just simply invert it back.
        StoryPresentaionScript.Open(currInteractable.items[InteractableID]);
    }
}
