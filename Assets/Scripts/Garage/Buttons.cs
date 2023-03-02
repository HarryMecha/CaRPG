using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    /*
     * The Buttons script is used to control the onClick actions of the buttons within the GarageScene, being able to update the current show car model in the scene and UI elements on button click.
     */

    #region Fields
    public garageSlot currentSlot;
    public int arrayMax;
    public GaragePanelConfig garagePanel;
    public GameObject GlobalHolder;
    public enum slotType
    {
        BODY,
        WHEEL,
        SPOILER
    }
    public slotType currentSlotType;
    #endregion

    private void Start()
    {
        garagePanel = GameObject.Find("ShopPanel").GetComponent<GaragePanelConfig>();
        arrayMax = currentSlot.getPartArraySize();
        GlobalHolder = GameObject.Find("GlobalHolder");
    }
    

    /*
     * onLeftButtonClick is a method used to handle when the left button is clicked on a garageSlot, ths method uses a case statement and an enum to determine which type of carPart is stored in the slot
     * and which array of the garage slot to change, the array contains all carParts of that specific type, it will check whether or not the part resides in the players inventory, if it does it will update
     * the UI to the new part is not it will remain locked same goes for the showCar GameObject in the scene.
     */
    public void onLeftButtonClick()
    {
        switch (currentSlotType)
        {
            case (slotType.BODY):
                if (garagePanel.currentBodyPartIndex == 0)
                {
                    garagePanel.currentBodyPartIndex = arrayMax;

                }
                garagePanel.currentBodyPartIndex--;
                GlobalHolder.GetComponent<PlayerStatHandler>().setCurrentBody(currentSlot.getPartArray(garagePanel.currentBodyPartIndex));
                garagePanel.shopSetup();
                GlobalHolder.GetComponent<PlayerStatHandler>().changeCar();
                break;
            case (slotType.WHEEL):
                if (garagePanel.currentWheelPartIndex == 0)
                {
                    garagePanel.currentWheelPartIndex = arrayMax;

                }
                garagePanel.currentWheelPartIndex--;
                GlobalHolder.GetComponent<PlayerStatHandler>().setCurrentWheels(currentSlot.getPartArray(garagePanel.currentWheelPartIndex));
                garagePanel.shopSetup();
                GlobalHolder.GetComponent<PlayerStatHandler>().changeCar();
                break;
            case (slotType.SPOILER):
                if (garagePanel.currentSpoilerPartIndex == 0)
                {
                    garagePanel.currentSpoilerPartIndex = arrayMax;

                }
                garagePanel.currentSpoilerPartIndex--;
                GlobalHolder.GetComponent<PlayerStatHandler>().setCurrentSpoiler(currentSlot.getPartArray(garagePanel.currentSpoilerPartIndex));
                garagePanel.shopSetup();
                GlobalHolder.GetComponent<PlayerStatHandler>().changeCar();
                break;
        }

    }

    /*
    * onRightButtonClick is a method used to handle when the right button is clicked on a garageSlot, ths method uses a case statement and an enum to determine which type of carPart is stored in the slot
    * and which array of the garage slot to change, the array contains all carParts of that specific type, it will check whether or not the part resides in the players inventory, if it does it will update
    * the UI to the new part is not it will remain locked same goes for the showCar GameObject in the scene.
    */
    public void onRightButtonClick()
    {
        switch (currentSlotType)
        {
            case (slotType.BODY):
                if (garagePanel.currentBodyPartIndex == arrayMax -1)
                {
                    garagePanel.currentBodyPartIndex = 0;

                }
                else
                {
                    garagePanel.currentBodyPartIndex++;
                }
                GlobalHolder.GetComponent<PlayerStatHandler>().setCurrentBody(currentSlot.getPartArray(garagePanel.currentBodyPartIndex));
                garagePanel.shopSetup();
                GlobalHolder.GetComponent<PlayerStatHandler>().changeCar();
                break;
            case (slotType.WHEEL):
                if (garagePanel.currentWheelPartIndex == arrayMax - 1)
                {
                    garagePanel.currentWheelPartIndex = 0;

                }
                else
                {
                    garagePanel.currentWheelPartIndex++;
                }
                GlobalHolder.GetComponent<PlayerStatHandler>().setCurrentWheels(currentSlot.getPartArray(garagePanel.currentWheelPartIndex));
                garagePanel.shopSetup();
                GlobalHolder.GetComponent<PlayerStatHandler>().changeCar();
                break;
            case (slotType.SPOILER):
                if (garagePanel.currentSpoilerPartIndex == arrayMax - 1)
                {
                    garagePanel.currentSpoilerPartIndex = 0;

                }
                else
                {
                    garagePanel.currentSpoilerPartIndex++;
                }
                GlobalHolder.GetComponent<PlayerStatHandler>().setCurrentSpoiler(currentSlot.getPartArray(garagePanel.currentSpoilerPartIndex));
                garagePanel.shopSetup();
                GlobalHolder.GetComponent<PlayerStatHandler>().changeCar();
                break;
        }

    }
}
