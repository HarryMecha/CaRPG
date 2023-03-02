using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSaveStateSlots : MonoBehaviour
{
    /*
     *  LoadSaveStates sets up the GUI for the SaveStateSelectPanel for the user to chose a SaveState to select when playing the game.
     */

    #region Fields
    DatabaseConfig config;
    private GameObject[] saveSlots;
    public GameObject SaveStateSelectPanel;
    #endregion


    private void Start()
    {
        config = GameObject.Find("GlobalHolder").GetComponent<DatabaseConfig>();
       saveSlots = GameObject.FindGameObjectsWithTag("SaveSlot");
        SaveStateSelectPanel.SetActive(false);
    }

    /*
     *  LoadSlots() sets the SaveStateSelectPanel to visible and sets each of the slot GameObject children within it's text and images based on if a saveSlot was found in the database with a 
     *  corresponding SlotID.
     */
    public void LoadSlots()
    {
        SaveStateSelectPanel.SetActive(true);
        foreach (GameObject slot in saveSlots)
        {
           SaveStateSlot saveSlot = slot.GetComponent<SaveStateSlot>();
           bool recordExists = config.SlotChecker(saveSlot.getSlotID());
            if (recordExists)
            {
                slot.GetComponent<SaveStateSlot>().setSaveExists(true);
                slot.GetComponent<SaveStateSlot>().setSlotImage(recordExists);
                slot.GetComponent<SaveStateSlot>().setSlotName("Continue");
            }
            else
            {
                slot.GetComponent<SaveStateSlot>().setSaveExists(false);
                slot.GetComponent<SaveStateSlot>().setSlotImage(recordExists);
                slot.GetComponent<SaveStateSlot>().setSlotName("New Game");
            }
        }
        GameObject.Find("GameSelectPanel").SetActive(false);
    }
}
