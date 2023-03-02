using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GaragePanelConfig : MonoBehaviour
{
    /*
     * The GaragePanelConfig controls all the UI of the GarageScene.
    */
    #region Fields
    private GameObject ShopPanel;
    private GameObject BodySlot, WheelSlot, SpoilerSlot,GlobalHolder;
    private GameObject PartIcon;
    public int currentBodyPartIndex, currentWheelPartIndex, currentSpoilerPartIndex;
    public GameObject currentBodyPart, currentWheelPart, currentSpoilerPart;
    public GameObject currentBodyPartMesh;
    public CarChanger carChanger;
    public GameObject LeaveButton;
    private bool anyLocked;
    #endregion

    void Start()
    {
        GlobalHolder = GameObject.Find("GlobalHolder");
        ShopPanel = GameObject.Find("ShopPanel");
        BodySlot = GameObject.Find("BodySlot");
        WheelSlot = GameObject.Find("WheelSlot");
        SpoilerSlot = GameObject.Find("SpoilerSlot");
        carChanger = GetComponent<CarChanger>();


        for (int i = 0; i < BodySlot.GetComponent<garageSlot>().getPartArraySize(); i++)
        {
            if(GlobalHolder.GetComponent<PlayerStatHandler>().getCurrentBody() == BodySlot.GetComponent<garageSlot>().getPartArray(i))
            {
                currentBodyPartIndex = i;
            }
            if (GlobalHolder.GetComponent<PlayerStatHandler>().getCurrentWheels() == WheelSlot.GetComponent<garageSlot>().getPartArray(i))
            {
                currentWheelPartIndex = i;
            }
            if (GlobalHolder.GetComponent<PlayerStatHandler>().getCurrentSpoiler() == SpoilerSlot.GetComponent<garageSlot>().getPartArray(i))
            {
                currentSpoilerPartIndex = i;
            }
        }
        shopSetup();
        anyLocked = false;
    }

    void Update()
    {
        if (anyLocked)
        {
            LeaveButton.GetComponent<Button>().enabled = false;
            LeaveButton.transform.Find("LockedIcon").GetComponent<Image>().enabled = anyLocked;
        }
        else
        {
            LeaveButton.GetComponent<Button>().enabled = true;
            LeaveButton.transform.Find("LockedIcon").GetComponent<Image>().enabled = anyLocked;
        }
    }

    /*
     * shopSetup() checks whether any of the setup for the parts has returned true.
     */
    public void shopSetup()
    {
        isLocked(bodySetup(), wheelSetup(), spoilerSetup());
    }

    /*
     * The bodySetup method manages everything related to the body of the car, the UI, the showCar model, whether it's locked by accessing the database to see if the part in the slot is within
    * the carParts table with a corresponding userID found in the PlayerStatHolder and returns that value to be used in isLocked().
    */
    public bool bodySetup()
    {
        currentBodyPart = BodySlot.GetComponent<garageSlot>().getPartArray(currentBodyPartIndex);
        CarParts currentBodyParts = currentBodyPart.GetComponent<CarParts>();
        carChanger.ChangeBody(currentBodyPart.GetComponent<CarParts>().getPartMesh());
        BodySlot.transform.Find("PartIcon").GetComponent<Image>().sprite = currentBodyParts.getIcon();
        BodySlot.transform.Find("LockedIcon").GetComponent<Image>().enabled = currentBodyParts.getIsLocked();
        if (currentBodyParts.getIsLocked())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     * The wheelSetup method manages everything related to the wheels of the car, the UI, the showCar model, whether it's locked by accessing the database to see if the part in the slot is within
    * the carParts table with a corresponding userID found in the PlayerStatHolder and returns that value to be used in isLocked().
    */
    public bool wheelSetup()
    {
        currentWheelPart = WheelSlot.GetComponent<garageSlot>().getPartArray(currentWheelPartIndex);
        CarParts currentWheelParts = currentWheelPart.GetComponent<CarParts>();
        carChanger.ChangeWheels(currentWheelPart.GetComponent<CarParts>().getPartMesh());
        WheelSlot.transform.Find("PartIcon").GetComponent<Image>().sprite = currentWheelParts.getIcon();
        WheelSlot.transform.Find("LockedIcon").GetComponent<Image>().enabled = currentWheelParts.getIsLocked();
        if (currentWheelParts.getIsLocked())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
    * The spoilerSetup method manages everything related to the spoiler of the car, the UI, the showCar model, whether it's locked by accessing the database to see if the part in the slot is within
    * the carParts table with a corresponding userID found in the PlayerStatHolder and returns that value to be used in isLocked().
   */
    public bool spoilerSetup()
    {
        currentSpoilerPart = SpoilerSlot.GetComponent<garageSlot>().getPartArray(currentSpoilerPartIndex); 
        CarParts currentSpoilerParts = currentSpoilerPart.GetComponent<CarParts>();
        carChanger.ChangeSpoiler(currentSpoilerPart.GetComponent<CarParts>().getPartMesh());
        SpoilerSlot.transform.Find("PartIcon").GetComponent<Image>().sprite = currentSpoilerParts.getIcon();
        SpoilerSlot.transform.Find("LockedIcon").GetComponent<Image>().enabled = currentSpoilerParts.getIsLocked();
        if (currentSpoilerParts.getIsLocked())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     * isLocked is a setter method for anyLocked, it is true when any of the car part prefabs in the garageSlot are not within the database, denying the user access back to the OverworldScene.
     */
    private void isLocked(bool bodyLock, bool wheelLock, bool spoilLock)
    {
        if(bodyLock == true || wheelLock == true || spoilLock == true)
        {
            anyLocked = true;
        }
        else
        {
            anyLocked = false;
        }
    }
    
       
    

}
