using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarChanger : MonoBehaviour
{
    /*
     * The CarChanger Script is used to change the parts of the player's car in relation to outside influence within the game.
     */

    #region Fields
    private GameObject Car,CarBody,CarWheels,CarSpoiler,CarWheelColliders;
    private Vector3 CarBodyPosition;
    private Vector3 CarWheelsPosition;
    private Vector3 CarSpoilerPosition;
    #endregion

    private void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void Update()
    {
        
     FindParts();

        
    }

    /*
     * The ChangeBody method will find the current gameobject with the body tag attached to it and replace it with a prefab set within the parameter, it will do this by instantiating
     * a new GameObject then attaching that to the gameobject with the player tag and then delete the original CarBody GameObject.
     */
    public void ChangeBody(GameObject bodyPrefab)
    {
        Car = GameObject.FindWithTag("Player");
        CarBody = GameObject.FindWithTag("Body");
        CarBodyPosition = CarBody.transform.position;
        Vector3 GlobalCarPosition = transform.TransformVector(CarBodyPosition);
        Quaternion currentRotate = new Quaternion();
        GameObject newCarBody = Instantiate(bodyPrefab, GlobalCarPosition, currentRotate);
        GameObject[] duplicateBody = GameObject.FindGameObjectsWithTag("Body");
        if (duplicateBody.Length != 1)
        {
            DeleteDuplicates(duplicateBody);
        }
        CarBody = newCarBody;
        CarBodyPosition = newCarBody.transform.position;
        CarBody.transform.parent = Car.transform;
        CarBody.transform.rotation = currentRotate;
       
    }

    /*
     * The ChangeWheels method will find the current gameobject with the wheels tag attached to it and replace it with a prefab set within the parameter, it will do this by instantiating
     * a new GameObject then attaching that to the gameobject with the player tag and then delete the original CarWheels GameObject.
     */
    public void ChangeWheels(GameObject wheelsPrefab)
    {
        
        CarWheels = GameObject.FindWithTag("Wheels");
        CarWheelsPosition = CarWheels.transform.position;
        Vector3 GlobalCarPosition = transform.TransformVector(CarWheelsPosition);
        GameObject newCarWheels = Instantiate(wheelsPrefab, GlobalCarPosition, new Quaternion()) as GameObject;
        GameObject[] duplicateWheels = GameObject.FindGameObjectsWithTag("Wheels");
        if (duplicateWheels.Length != 1)
        {
            DeleteDuplicates(duplicateWheels);
        }
        CarWheels = newCarWheels;
        CarWheelsPosition = newCarWheels.transform.position;
        CarWheels.transform.parent = Car.transform;
        CarWheels.transform.rotation = new Quaternion();

    }

    /*
     * The ChangeSpoiler method will find the current gameobject with the spoiler tag attached to it and replace it with a prefab set within the parameter, it will do this by instantiating
     * a new GameObject then attaching that to the gameobject with the player tag and then delete the original CarSpoiler GameObject.
     */
    public void ChangeSpoiler(GameObject spoilerPrefab)
    {
        CarSpoiler = GameObject.FindWithTag("Spoiler");
        CarSpoilerPosition = CarSpoiler.transform.position;
        Vector3 GlobalCarPosition = transform.TransformVector(CarSpoilerPosition);
        GameObject newCarSpoiler = Instantiate(spoilerPrefab, GlobalCarPosition, new Quaternion()) as GameObject;
        GameObject[] duplicateSpoiler = GameObject.FindGameObjectsWithTag("Spoiler");
        if (duplicateSpoiler.Length != 1)
        {
            DeleteDuplicates(duplicateSpoiler);
        }
        CarSpoiler = newCarSpoiler;
        CarSpoilerPosition = newCarSpoiler.transform.position;
        CarSpoiler.transform.parent = Car.transform;
        CarSpoiler.transform.rotation = new Quaternion();
    }

    /*
     * FindParts() will find all the current GameObject with specific tags relating to the player's car, this is called when a new scene is loaded and when any change is made to the car.
     */
    public void FindParts()
    {
        Car = GameObject.FindWithTag("Player");
        CarBody = GameObject.FindWithTag("Body");
        CarWheels = GameObject.FindWithTag("Wheels");
        CarSpoiler = GameObject.FindWithTag("Spoiler");
        CarWheelColliders = GameObject.FindWithTag("WheelColliders");
    }

    /*
     * ChangeActiveScene() is an inbuilt function in the Unity.SceneManagment library I am using it here to call the FindParts method in order to assign each GameObject field with the correct
     * tagged GameObject within that scene following a scene transition.
     */
    public void ChangedActiveScene(Scene prevScene, Scene newScene)
    {
        FindParts();
        
    }

    /*
     * DeleteDuplicates() is a method that will take in an array of GameObjects found by the FindGameObjectsWithTag() method and then delete each one up to just before the array's length, the index
     * value of the most recently instantiated GameObject.
     */
    private void DeleteDuplicates(GameObject[] gameObjects)
    {
        for (int i = 0; i < gameObjects.Length -1; i++)
        {
                Destroy(gameObjects[i]);
           
        }
    }

}
