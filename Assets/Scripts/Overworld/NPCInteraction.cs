using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCInteraction : MonoBehaviour
{
    /*
     * NPCInteraction script manages everything that happens when interacting with an NPC, UI, colliders, cameras etc.
     */
    
    #region Fields
    [SerializeField]
    public GameObject NPCInteractionPanel, CameraPosition;
    public NPCStats npcStats;
    public bool firstTimeInteraction;
    private GameObject NPCOptionPanel, NPCName, NPCDialouge, mainCamera, Player, Pyramid, ShopPanel;
    private int dialougePage;
    public static bool isInteracting, inCollider;
    #endregion
    void Start()
    { 
        npcStats = GetComponent<NPCStats>();
        isInteracting = false; 
        NPCOptionPanel = GameObject.Find("NPCOptionPanel");
        NPCName = GameObject.Find("NPCNameText");
        NPCDialouge = GameObject.Find("NPCDialouge");
        mainCamera = GameObject.FindWithTag("MainCamera");
        Player = GameObject.FindWithTag("Player");
        Pyramid = transform.Find("Pyramid").gameObject;
        ShopPanel = GameObject.Find("ShopMenu");
        Pyramid.SetActive(false);
        dialougePage = -1;
        NPCInteractionPanel.SetActive(false);
        ShopPanel.SetActive(false);
    }

    /*
     * The Update() waits and reacts for player inputs.
     */

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)&&isInteracting)
        {
            ChangeDialougeText();
        }
        if (inCollider)
        {
            if (Input.GetKeyDown(KeyCode.E)&&(isInteracting == false)&&(inCollider == true)) { Pyramid.SetActive(false); isInteracting = true; PanelSetup(); StartCoroutine(changeCamera(mainCamera, CameraPosition));}

            if (Input.GetKeyDown(KeyCode.Escape)) {
                CloseMenu();
            } 
        }
    }

    /*
     * OnTriggerEnter() makes sure the GameObject that entered has the tag player and then sets active the pyramid object that lets the player visually know it's within the collider and sets the
     * bool inCollider to be true to let the script know it can run code that requires the player to be inside when the player GameObject is set to inactive so it doesn't bloack camer view.
     */
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player")) 
        {
            Pyramid.SetActive(true);
            inCollider = true;
           
        }

    }

    /*
     * OnTriggerClose() makes sure the GameObject that entered has the tag player and then sets inactive the pyramid object that lets the player visually know it's not within the collider and sets the
     * bool inCollider to be false to let the script know it stop runing code that requires the player to be inside.
     */
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            Pyramid.SetActive(false);
            inCollider = false;

        }

    }

    /*
     * PanelSetup() sets up the UI, stopping the cameraFollow script to reposition the camera, sets the player GameObject to inactive so it's out of view and turns on any GUI Panel for the user.
     */
    void PanelSetup()
    {
        NPCInteractionPanel.SetActive(true);
        NPCOptionPanel.SetActive(false);
        mainCamera.GetComponentInChildren<CameraFollow>().enabled = false;
        Player = GameObject.FindWithTag("Player");
        Player.SetActive(false);
        string NPCNameText = npcStats.getNpc_name();
        NPCName.GetComponent<TextMeshProUGUI>().SetText(NPCNameText);
        ChangeDialougeText();
    }

    /*
     * ChangeDialougeText() manages the text displayed in the GUI to the values set within the NPCStats class of the NPC you are interacting with, including whether it's your first time interacting
     * with the NPC and which page of dialouge they are on and then opens the options menu when all dialouge has been display.
     */
    void ChangeDialougeText()
    {
        int maxDialougePage = npcStats.getNpc_DialougeLength();
        if (dialougePage < maxDialougePage -1 && firstTimeInteraction)
        {
            dialougePage++;
        }
        else
        {
            firstTimeInteraction = false;
            openOptionsPanel();
        }
        string NPCDialougeText = npcStats.getNpc_Dialouge(firstTimeInteraction,dialougePage);
        NPCDialouge.GetComponent<TextMeshProUGUI>().SetText(NPCDialougeText);    
    }

    /*
     * openOptionsPanel() manages the optionPanel, opening it and set the options to those that correspond in the NPC's NPCStats class.
     */
    void openOptionsPanel()
    {
        NPCOptionPanel.SetActive(true);
        string[] NPCOptions = npcStats.getnpc_options();
        for (int i = 0; i < NPCOptions.Length; i++)
        {
            GameObject.Find("NPCOption"+(i+1)).GetComponent<TextMeshProUGUI>().SetText(NPCOptions[i]);
        }
    }

    /*
     * changeCamera() is a coroutine that when running lerps the camera towards it's new position when interacting with an NPC.
     */
    IEnumerator changeCamera(GameObject mainCamera, GameObject newCameraPosition)
    {

        while ((isInteracting) &&(mainCamera.transform.position != newCameraPosition.transform.position))
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newCameraPosition.transform.position, 5.0f * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, newCameraPosition.transform.rotation, 5.0f * Time.deltaTime);
            yield return null;
        }

    }

    /*
     * CloseMenu() closes the GUI elements in the menu when a button is clicked.
     */
    public void CloseMenu()
    {
        Pyramid.SetActive(true);
        isInteracting = false;
        NPCInteractionPanel.SetActive(false);
        ShopPanel.SetActive(false);
        mainCamera.GetComponentInChildren<CameraFollow>().enabled = true;
        Player.SetActive(true);
        firstTimeInteraction = false;
    }

    /*
     * OpenShop() opens up the shop GUI and runs the shopInteraction script that is held on the NPC GameObject.
     */
    public void OpenShop()
    {
        NPCInteractionPanel.SetActive(false);
        ShopPanel.SetActive(true);
        GetComponent<ShopInteraction>().shopSetup();
    }

    /*
     * turnOffInCollider() setter method for inCollider called by buttons when the scene needs to be changed to make sure the system doesn't still think the player is in a collider on scene reload.
     */
    public void turnOffInCollider()
    {
        inCollider = !inCollider;
    }
}
