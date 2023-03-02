using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarDrive : CarDriver
{

    /*
     * EnemyCarDrive handles the enemy car GameObjects movement towards certain checkpoints on the map and inherits from CarDriver.
     */

    #region Fields
    public Transform currentCheckpointTransform;
    private Vector3 currentCheckpointPosition, direction;
    private RaceManager raceManager;
    public int CheckpointID;
    public int LapNumber;
    private float dotProductToCheckpoint;
    #endregion

    
    void Start()
    {
        raceManager = GameObject.Find("GlobalHolder").GetComponent<RaceManager>();
        CheckpointID = 0;
        LapNumber = 0;
        setNextPosition(CheckpointID);
    }

    /*
     *  We use FixedUpdated as that is defined in the editor and if better for physics and will not be susceptible to frame rate drops or increases as this is where all of
     *  the calculations for the enemy car's direction, it will find it's distance to the next checkpoint, normalised, and then calulcate the dot product between itself and the checkpoint,
     *  if it's behind it, it will drive forward else it will drive backwards, the angle to the checkpoint is also calulated by SignedAngle, which is the angle of rotation between the first
     *  parameter vector and the other, this will then set the angle the car must be driving at any time to get to the checkpoint.
    */
    void FixedUpdate()
    {
        AccelerationHandler();
        TurnHandler();
        WheelRotationHandler();
        float forwardAmount = 0f;
        float turnAmount = 0f;

         direction = (currentCheckpointPosition - transform.position).normalized;
         dotProductToCheckpoint = Vector3.Dot(transform.forward, direction);

        if(dotProductToCheckpoint > 0)
        {
            forwardAmount = 1f;
        } else
        {
            forwardAmount = -1f;
        }

        float angletoDIR = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

        if (angletoDIR > 0)
        {
            turnAmount = 1f;
        }
        else
        {
            turnAmount = -1f;
        }

        if((transform.position.x == currentCheckpointPosition.x)||(transform.position.z == currentCheckpointPosition.z))
        {
            currentCheckpointPosition = raceManager.getCurrentCheckpointPos(CheckpointID);
        }


        SetEnemyInputs(forwardAmount, turnAmount);
    }

    /*
     * setNextPosition() takes the current checkpoint ID and then calculates a random point within the collider of the next checkpoint and sets that to the destination position.
     */
    public void setNextPosition(int ID)
    {
        CheckpointID = ID;
        currentCheckpointPosition = raceManager.getCurrentCheckpointPosEnemy(ID);
    }

    /*
     * setNextLap() setter method for LapNumber, increases it by 1 when called.
     */
    public void setNewLap()
    {
        LapNumber++;
    }

    /*
     * getDot() getter method for dotProductToCheckpoint, used to compare how close the player is to the checkpoint compared to the enemy by comparing dot products.
     */
    public float getDot()
    {
        return dotProductToCheckpoint;
    }
}
