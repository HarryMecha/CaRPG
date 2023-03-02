using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceSignInteraction : MonoBehaviour
{
    /*
     * NPCInteraction script manages everything that happens when interacting with a RaceSign, UI, colliders, cameras etc.
     */
    #region Fields
    public GameObject CameraPosition, timeTrialButton, normalRaceButton;
    private GameObject  mainCamera, Player, Pyramid, RaceSelectMenu;
    public TextMeshProUGUI bestTime;
    private GameObject GlobalHolder;
    public GameObject[] difficultyButtons;
    public static bool isInteracting, inCollider;
    #endregion

    void Start()
    {
        GlobalHolder = GameObject.Find("GlobalHolder");
        RaceSelectMenu = GameObject.Find("RaceSelectMenu");
        RaceSelectMenu.SetActive(false);
        isInteracting = false;
        mainCamera = GameObject.FindWithTag("MainCamera");
        Player = GameObject.FindWithTag("Player");
        Pyramid = transform.Find("Pyramid").gameObject;
        Pyramid.SetActive(false);
        inCollider = false;
    }


    /*
     * The Update() waits and reacts for player inputs also setting up each of the difficulty button's onClick events when the E key clicked.
     */
    private void Update()
    {
        if (inCollider)
        {
            if (Input.GetKeyDown(KeyCode.E) && (isInteracting == false)) { PanelSetup();  Pyramid.SetActive(false); isInteracting = true; StartCoroutine(changeCamera(mainCamera, CameraPosition));

                Button currentButton = difficultyButtons[0].GetComponent<Button>();
                currentButton.onClick.AddListener(delegate { GlobalHolder.GetComponent<RaceManager>().setDifficulty(1); });
                currentButton = difficultyButtons[1].GetComponent<Button>();
                currentButton.onClick.AddListener(delegate { GlobalHolder.GetComponent<RaceManager>().setDifficulty(2); });
                currentButton = difficultyButtons[2].GetComponent<Button>();
                currentButton.onClick.AddListener(delegate { GlobalHolder.GetComponent<RaceManager>().setDifficulty(3); });
                currentButton = timeTrialButton.GetComponent<Button>();
                currentButton.onClick.AddListener(delegate { GlobalHolder.GetComponent<RaceManager>().setTimeTrial(true); });
                currentButton = normalRaceButton.GetComponent<Button>();
                currentButton.onClick.AddListener(delegate { GlobalHolder.GetComponent<RaceManager>().setTimeTrial(false); });
                float seconds = GlobalHolder.GetComponent<PlayerStatHandler>().bestTime;
                int minutes = 0;
                if (GlobalHolder.GetComponent<PlayerStatHandler>().bestTime > 60)
                {
                     minutes = (int)GlobalHolder.GetComponent<PlayerStatHandler>().bestTime / 60;
                    seconds = seconds - (60 * minutes);
                }
                bestTime.text = minutes.ToString("00")+":"+seconds.ToString("00.00");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
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
        RaceSelectMenu.SetActive(true);
        mainCamera.GetComponentInChildren<CameraFollow>().enabled = false;
        Player = GameObject.FindWithTag("Player");
        Player.SetActive(false);

    }
    /*
     * CloseMenu() closes the GUI elements in the menu when a button is clicked.
     */
    public void CloseMenu()
    {
        Pyramid.SetActive(true);
        RaceSelectMenu.SetActive(false);
        isInteracting = false;
        mainCamera.GetComponentInChildren<CameraFollow>().enabled = true;
        Player.SetActive(true);
    }
    /*
     * changeCamera() is a coroutine that when running lerps the camera towards it's new position when interacting with an NPC.
     */
    IEnumerator changeCamera(GameObject mainCamera, GameObject newCameraPosition)
    {

        while ((isInteracting) && (mainCamera.transform.position != newCameraPosition.transform.position))
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newCameraPosition.transform.position, 5.0f * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, newCameraPosition.transform.rotation, 5.0f * Time.deltaTime);
            yield return null;
        }

    }
    /*
     * turnOffInCollider() setter method for inCollider called by buttons when the scene needs to be changed to make sure the system doesn't still think the player is in a collider on scene reload.
     */
    public void turnOffInCollider()
    {
        inCollider = !inCollider;
    }

}
