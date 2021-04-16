using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryPopup : MonoBehaviour
{
    // UI
    public GameObject interactableArea;

    public Priest priest;

    private static Dictionary<string, bool> CheckedStage;

    public bool IsInvestigating
    {
        get; private set;
    }

    private Dictionary<string, bool> GetCheckedStage()
    {
        if (CheckedStage == null)
        {
            CheckedStage = new Dictionary<string, bool>() 
            {
                { "Bible", false},
                { "WoodenChest", false},
                { "Monk_Robes", false},
                { "PrayerKneeler", false},
                { "WeaponCloset", false}
            };
        }
        return CheckedStage;
    }

    private Interactable currInteractable;
    private string currInteractableName;

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
        currInteractableName = obj.name;

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

        Dictionary<string, bool> checkedStage = GetCheckedStage();
        if (checkedStage[currInteractableName])
        {
            priest.IncrementStage();
        }
    }

    public void OnInteractableSelected(int InteractableID)
    {
        StoryPresentation StoryPresentationScript = FindObjectOfType<StoryPresentation>();

        // A little fuggly here as the InteractableID inverst because of the Grid Layout Group
        // attached to the Interactable area. It turns out that as you add items it inverts
        // the Interactable ID. Its okay now that it is unsderstood we just simply invert it back.
        StoryPresentationScript.Open(currInteractable.items[InteractableID]);

        // register the callback
        StoryPresentationScript.StoryItemClicked += OnStoryItemClickedHandler;
    }

    private void OnStoryItemClickedHandler(object src, StoryItemClickedEventArgs args)
    {
        Dictionary<string, bool> checkedStage = GetCheckedStage();
        checkedStage[currInteractableName] = true;
    }
}
