using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    /*
     * This is the Checkpoint class that manages when an object interacts with a checkpoint.
     */

    #region Fields
    public int ID;
    public bool isPassed;
    private GameObject globalHolder;
    #endregion

    private void Start()
    {
        globalHolder = GameObject.Find("GlobalHolder");
        
    }

    /*
    *  OnTrigerEnter() when the checkpoint is passed it will call the relevant methods to update the players current position and if an enemy passed set their new checkpoint or
    *  lap number if necessary.
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (globalHolder.GetComponent<RaceManager>().getCurrentCheckpoint() == (ID - 1)&& (isPassed == false))
            {
                isPassed = true;
                globalHolder.GetComponent<RaceManager>().setCurrentCheckpoint(ID);
            }
        }

        if (other.gameObject.tag == "Enemy")
        {
            if (ID == (other.gameObject.GetComponent<EnemyCarDrive>().CheckpointID))
            {
                if (ID == 8)
                {
                    
                    other.gameObject.GetComponent<EnemyCarDrive>().setNextPosition(0);
                    other.gameObject.GetComponent<EnemyCarDrive>().setNewLap();
                }
                else
                {
                    other.gameObject.GetComponent<EnemyCarDrive>().setNextPosition(ID + 1);
                }

            }
            
        }
    }


    /*
     * resetisPassed() setter method for isPassed, it resets isPassed back to false when a lap is complete.
     */
    public void resetisPassed()
    {
        isPassed = false;
    }

    /*
     * getID() getter method for ID, returns the checkpoint ID.
     */
    public int getID()
    {
        return ID;
    }
}
