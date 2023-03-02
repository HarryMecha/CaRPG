using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ShopInteraction : MonoBehaviour
{
    /*
     * ShopInteraction script manages everything that happens when interacting with an NPC's Shop, UI, colliders, cameras etc.
     */
    #region Fields
    public NPCStats stats;
    public GameObject shopMenu,shopPreviewPanel;
    private PlayerStatHandler playerStats;
    public TextMeshProUGUI ShopText;
    public TextMeshProUGUI ShopNameText;
    public GameObject[] shopItemSlots;
    public TextMeshProUGUI ShopCharacterMoneyValue;
    private GameObject selectedItem;
    public Sprite EmptyItem;
    #endregion

    /*
     * shopSetup() manages the UI for the shop menu, setting up it's elements with the corresponding values, each of the shopItemSlots holds a prefab of a carPart and is displayed as such, the 
     * shop name and dialouge is retrieved from the NPC'c NPCstats class and the gold amount from the player's PlayerStatHolder class.
     */
    public void shopSetup()
    {
        playerStats = GameObject.Find("GlobalHolder").GetComponent<PlayerStatHandler>();
        ShopText.text = stats.getNpc_shopDialouge();
        ShopNameText.text = stats.getNpc_name();
        shopItemSlots = GameObject.FindGameObjectsWithTag ("shopItemSlots");
        ShopCharacterMoneyValue.text = playerStats.money.ToString();
        for (int i = 0; i < shopItemSlots.Length; i++)
        {
            CarParts currentCarPart = shopItemSlots[i].GetComponent<shopSlot>().CarPart.GetComponent<CarParts>();
                shopItemSlots[i].GetComponent<shopSlot>().isSold = !currentCarPart.getIsLocked();
            if (shopItemSlots[i].GetComponent<shopSlot>().isSold)
            {
                shopItemSlots[i].GetComponent<TextMeshProUGUI>().text = "SOLD";
            }
            else
            {
                shopItemSlots[i].GetComponent<TextMeshProUGUI>().text = currentCarPart.name;
            }
        }

    }

    /*
     * PreviewPanelSetup() manages the Ui for the shops preview panel, setting the panel to display which ever GameObject has been selected by the corresponding button on the ShopItemSlot, checking whether
     * that value has is within the carParts table of the database with the players corresponding userID, if it's sold it will display as much if not it will retreive that data for the part from it's
     * CarParts script and display it on the GUI.
     */
    public void PreviewPanelSetup()
    {
        CarParts currentCarPart = selectedItem.GetComponent<shopSlot>().CarPart.GetComponent<CarParts>();
        if (selectedItem.GetComponent<shopSlot>().isSold)
        {
            selectedItem.GetComponent<TextMeshProUGUI>().text = "SOLD";
            shopPreviewPanel.transform.Find("ItemPreviewName").GetComponent<TextMeshProUGUI>().text = "SOLD";
            shopPreviewPanel.transform.Find("ItemPreviewPrice").GetComponent<TextMeshProUGUI>().text = "SOLD";
            GameObject.Find("ItemSlotImage").GetComponent<Image>().sprite = EmptyItem;

        }
        else
        {
            selectedItem.GetComponent<TextMeshProUGUI>().text = currentCarPart.name;
            shopPreviewPanel.transform.Find("ItemPreviewName").GetComponent<TextMeshProUGUI>().text = currentCarPart.name;
            shopPreviewPanel.transform.Find("ItemPreviewPrice").GetComponent<TextMeshProUGUI>().text = "Price: "+currentCarPart.BuyAmount.ToString() + "g";
            shopPreviewPanel.transform.Find("ItemPreviewDescription").GetComponent<TextMeshProUGUI>().text = currentCarPart.Description;
            GameObject.Find("ItemSlotImage").GetComponent<Image>().sprite = currentCarPart.Icon;
        }
    }

    /*
     * PreviewPanelSetup() manages the Ui for the shops preview panel, setting the panel to display which ever GameObject has been selected by the corresponding button on the ShopItemSlot, checking whether
     * that value has is within the carParts table of the database with the players corresponding userID, if it's sold it will display as much if not it will retreive that data for the part from it's
     * CarParts script and display it on the GUI.
     */
    public void setSelectedItem(GameObject selectedItem)
    {
        this.selectedItem = selectedItem;
        PreviewPanelSetup();
    }

    /*
    * buyItem() runs on buttonClick, it checks whether the current item is already sold, if not it will check whether the player has the required amount of money and if so adds the name of the carPart into
    * the newUnlockParts array of playStats to be saved to the database.
    */
    public void buyItem()
    {
        if (selectedItem.GetComponent<shopSlot>().isSold == false)
        {
            if (playerStats.money >= selectedItem.GetComponent<shopSlot>().CarPart.GetComponent<CarParts>().BuyAmount)
            {
                playerStats.money -= selectedItem.GetComponent<shopSlot>().CarPart.GetComponent<CarParts>().BuyAmount;
                playerStats.addNewUnlockedPart(selectedItem.GetComponent<shopSlot>().CarPart.GetComponent<CarParts>().name);
                shopSetup();
                PreviewPanelSetup();
            }
        }
    }

}
