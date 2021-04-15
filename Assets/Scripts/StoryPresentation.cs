using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPresentation : MonoBehaviour
{
    private GameObject m_SelectPopUp;
    private GameObject m_StoryPopup;
    private StoryPopUpMgr m_StoryPopUpMgr;
    private int m_InteractableID;

    public void Open(int InteractableID)
    {
        // Get the Find SelectPopup object so we can change the GameObject active state
        m_SelectPopUp = this.GetComponent<Transform>().Find("SelectPopup").gameObject;

        // We want to get script but because its parent is inactive we can't find it directly.
        // To compensate for this we can find its parent and then dig out the component
        m_StoryPopup = this.GetComponent<Transform>().Find("StoryPopup").gameObject;
        m_StoryPopUpMgr = m_StoryPopup.GetComponent<StoryPopUpMgr>();

        // Don't forget to record the InteractiveID for later use to pass along to story
        m_InteractableID = InteractableID;

        // All done so show the activate the game object and as the hieracy is setup this
        // will show over the last window and is what we want.
        m_SelectPopUp.SetActive(true);
    }

    public void CloseSelectPopup()
    {
        //  On close click we just want to deactivate the popup
        m_SelectPopUp.SetActive(false);
    }

    public void OnStoryItemClick(int StoryItemID)
    {
        // Based on the story item clicked it will pass in the StoryItemID it is
        // associated with and we pass that on as an argument to the story popup
        // for proper story diplay.
        m_StoryPopUpMgr.Open(m_InteractableID, StoryItemID);
    }
}
