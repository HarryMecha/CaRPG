using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveStateSlot : MonoBehaviour
{
    /*
     * SaveStateSlot is the class for SaveStateSlot's it holds all the variables assigned with the class and the getter and setter methods and a method for the onClick of the button component attached
     * that will load or create a new SaveSlot in the database depending on whether a record already exists or not.
     */

    #region Fields
    public int slotID;
    public bool saveExists;
    #endregion

    /*
     * getSlotID() getter method for slotID.
     */
    public int getSlotID()
    {
        return slotID;
    }

    /*
     * getSaveExists() getter method for saveExists.
     */
    public bool getSaveExists()
    {
        return saveExists;
    }

    /*
     * setSaveExists() setter method for saveExists takes a boolean input from the DatabaseManager, and defines whether a userID is contained within the SaveStates table in the database.
     */
    public void setSaveExists(bool Boolean)
    {
        saveExists = Boolean;
    }

    /*
     * setSlotName() changes the text of the slotName GUI element.
     */
    public void setSlotName(string slotName)
    {
        transform.Find("SlotName").GetComponent<TextMeshProUGUI>().SetText(slotName);
    }

    /*
     * setSlotImage() changes the sprite of the slotImage GUI element.
     */
    public void setSlotImage(bool Boolean)
    {
        transform.Find("SlotImage").GetComponent<Image>().enabled = Boolean;
    }

    /*
     * ButtonGamePress() runs depending on whether a save with the userID corresponding to the slot already exists, if it does it sets the userID of the PlayerStatHandler and loads the entry in the Stats
     * table of the database whilst it is does not it will run the NewGame method which will create a new entry in the database with all default statistics.
     */
    public void ButtonGamePress()
    {
        if (saveExists)
        {
            GameObject.Find("GlobalHolder").GetComponent<PlayerStatHandler>().setUserID(slotID);
            GameObject.Find("GlobalHolder").GetComponent<PlayerStatHandler>().LoadFromDB();
        }
        else
        {
            DatabaseConfig.NewGame(slotID);
            GameObject.Find("GlobalHolder").GetComponent<PlayerStatHandler>().setUserID(slotID);
            GameObject.Find("GlobalHolder").GetComponent<PlayerStatHandler>().LoadFromDB();

        }
    }
}
