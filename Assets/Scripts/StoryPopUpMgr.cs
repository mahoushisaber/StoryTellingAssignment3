using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryPopUpMgr : MonoBehaviour
{
    public TMP_Text StoryTextTMPro;
    public Image StoryItemImage;
    public List<InteractableStoryItem> InteractableItems;


    // When called it uses the story ID passed in to select on of the 
    public void Open(Item interactableItem, int StoryItemID)
    {
        StoryTextTMPro.text = "No story availalbe for this selection";

        // Valid so lets set up the text box based on the items passed in
        switch (StoryItemID)
        {
            case 1:
                StoryTextTMPro.text = InteractableItems[interactableItem.itemID].StoryItem1;
                break;
            case 2:
                StoryTextTMPro.text = InteractableItems[interactableItem.itemID].StoryItem2;
                break;
            case 3:
                StoryTextTMPro.text = InteractableItems[interactableItem.itemID].StoryItem3;
                break;
        }

        // Assign the image for this interactable
        StoryItemImage.sprite = interactableItem.image;

        this.gameObject.SetActive(true);
    }

    // Button close function
    public void CloseStoryPopup()
    {
        StoryTextTMPro.text = "Story Text";
        StoryItemImage.sprite = null;
        this.gameObject.SetActive(false);
    }
}


[System.Serializable]
public struct InteractableStoryItem
{
    public string InteractableName;
    public string StoryItem1;
    public string StoryItem2;
    public string StoryItem3;
}

