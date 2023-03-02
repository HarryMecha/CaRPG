using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerStatHandler : MonoBehaviour
{
    /*
     * PlayerStatHandler is the script that holds all the values that the rest of the system needs to pull from regarding the player, including all of the stats and CarParts,
     * this is also the script that pulls from and pushes changes made to the stats into the database.
     */

    #region Fields
    private int userID;
    public GameObject currentBody, currentWheels, currentSpoiler, wheelColliders, player;
    public float speed, accelerate, weight;
    public Vector3 position;
    public Vector3 rotation;
    public int money;
    public float bestTime;
    public CarChanger carChanger;
    public Scene currentScene;
    public ArrayList newUnlockedParts;
    #endregion

    /*
     * On Awake any duplicates to PlayerStatHandler that can be found and destroyed.
     */

    private void Awake()
    {
        PlayerStatHandler[] duplicates = FindObjectsOfType<PlayerStatHandler>();
        if (duplicates.Length != 1)
        {
            Destroy(duplicates[1].gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.activeSceneChanged += ChangedActiveScene;
        newUnlockedParts = new ArrayList();

    }

    /*
     *  This object will be set to DontDestoryOnLoad as it will need to be pulled from in every scene.
     */
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /*
     *  The changeCar() method runs each of the change method for each part type of the car using the prefabs loaded from the inventory.
     */

    public void changeCar()
    {
        carChanger = GetComponent<CarChanger>();
        carChanger.ChangeBody(currentBody.GetComponent<CarParts>().getPartMesh());
        carChanger.ChangeWheels(currentWheels.GetComponent<CarParts>().getPartMesh());
        carChanger.ChangeSpoiler(currentSpoiler.GetComponent<CarParts>().getPartMesh());  
        
        
    }

    /*
     *  The LoadFromDB() method runs the StatLoader method in the DatabaseConfig script which populates the stats in this class with values from the database based on the current userID.
     */
    public void LoadFromDB()
    {
        DatabaseConfig.StatLoader(userID);
    }

    /*
     *  The SaveDB() method runs the StatUpdater method in the DatabaseConfig script which uses all of the stats in this class as parameters, plus adds any newly unlocked path from the ArrayList
     *  into the database and clears the arraylist, including the current position of the car on time of saving.
     */
    public void SaveDB() {

        position = player.transform.position;
        rotation = player.transform.rotation.eulerAngles;
        DatabaseConfig.unlockedPartsUpdater(userID, newUnlockedParts);
        newUnlockedParts.Clear();
        DatabaseConfig.StatUpdater(userID, position, rotation, money, bestTime, currentBody.GetComponent<CarParts>().partName, currentWheels.GetComponent<CarParts>().partName, currentSpoiler.GetComponent<CarParts>().partName);    
    }

    /*
     *  setCurrentBody() is a setter method for the currentBody GameObject taking in a GameObject as a parameter, the prefab for the CarPart.
     */
    public void setCurrentBody(GameObject BodyPrefab)
    {
        currentBody = BodyPrefab;
    }

    /*
     *  setCurrentWheels() is a setter method for the currentWheels GameObject taking in a GameObject as a parameter, the prefab for the CarPart.
     */
    public void setCurrentWheels(GameObject WheelsPrefab)
    {
        currentWheels = WheelsPrefab;
    }

    /*
     *  setCurrentSpoiler() is a setter method for the currentSpoiler GameObject taking in a GameObject as a parameter, the prefab for the CarPart.
     */
    public void setCurrentSpoiler(GameObject SpoilerPrefab)
    {
        currentSpoiler = SpoilerPrefab;
    }

    /*
     *  setUserID() is a setter method for the userID field.
     */
    public void setUserID(int ID)
    {
        userID = ID;
    }

    /*
     *  getUserID() is a getter method which returns the userID.
     */
    public int getUserID()
    {
        return userID;
    }

    /*
     *  setRotation() is a setter method for the rotation which gets the current rotation of the car.
     */
    public void setRotation()
    {
        rotation = transform.rotation.eulerAngles;
    }

    /*
     *  getNewUnlockedParts() is a getter method which returns an ArrayList of string names of the unlocked parts during that play session.
     */
    public ArrayList getNewUnlockedParts()
    {
        return newUnlockedParts;
    }

    /*
     *  addNewUnlockedParts() is a method that adds a new string set in the parameter to the newUnlockedParts ArrayList.
     */
    public void addNewUnlockedPart(string newPart)
    {
        newUnlockedParts.Add(newPart);
    }

    /*
     *  getCurrentBody() is a getter method which returns the currentBody prefab.
     */
    public GameObject getCurrentBody()
    {
        return currentBody;
    }

    /*
     *  getCurrentWheels() is a getter method which returns the currentWheels prefab.
     */
    public GameObject getCurrentWheels()
    {
        return currentWheels;
    }

    /*
     *  getCurrentSpoiler() is a getter method which returns the currentSpoiler prefab.
     */
    public GameObject getCurrentSpoiler()
    {
        return currentSpoiler;
    }

    /*
     *  getBestTime() is a getter method which returns bestTime, the best achieved time by the player in time trial.
     */
    public float getBestTime()
    {
        return bestTime;
    }

    /*
    * ChangedActiveScene is a function in the UnityEngine.SceneManagement library that runs when a new scene is loaded. This method in this class setups the player GameObject, it's position
    * rotation and car parts based on the stats and prefabs in this class.
    */
    public void ChangedActiveScene(Scene prevScene, Scene newScene)
    {
        currentScene = newScene;
        if(newScene.name != "StartScene")
        {
            transform.GetComponent<CarChanger>().enabled = true;
        player = GameObject.FindGameObjectWithTag("Player");
            if (newScene.name == "OverworldScene") { 
                player.transform.position = position;
                Quaternion newRotation = new Quaternion();
                newRotation.eulerAngles = rotation;
                player.transform.rotation = newRotation;
            }
            PlayerStatHandler[] duplicates = FindObjectsOfType<PlayerStatHandler>();
        if (duplicates.Length != 1)
        {
            if (this.gameObject == duplicates[0].gameObject)
            {
                changeCar();
            }
        }
        else
        {
            changeCar();
        }
        }
        else
        {
            transform.GetComponent<CarChanger>().enabled = false;
        }

        
        
    }

}
