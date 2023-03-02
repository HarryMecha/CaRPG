using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarDriver : MonoBehaviour {

    /*
     * The CarDriver script is used to control both the enemy's and player's car movement. 
   */

    #region Fields
    private float speed;
    public float speedMax = 20f;
    private float speedMin = -20f;
    public float acceleration = 3f;
    private float brakeSpeed = 100f;
    public float reverseSpeed = 3f;
    public float idleSlowdown = 10f;

    public float turnSpeed;
    private float turnSpeedMax = 300f;
    private float turnSpeedAcceleration = 300f;
    private float turnIdleSlowdown = 500f;

    private float forwardAmount;
    private float turnAmount;

    private Rigidbody carRigidbody;
    private Scene currentScene;
    private GameObject GlobalHolder;

    public WheelCollider frontLeftWheelCollider, frontRightWheelCollider, backLeftWheelCollider, backRightWheelCollider;
    public Transform frontLeftWheelTransform, frontRightWheelTransform, backLeftWheelTransform, backRightWheelTransform;
    #endregion

    private void Awake() {
        carRigidbody = GetComponent<Rigidbody>();
        GlobalHolder = GameObject.Find("GlobalHolder");
        currentScene = SceneManager.GetActiveScene();
        float totalSpeedMax = //Sets the totalSpeedMax to the current Speed values assigned to each of the car parts currently assigned to the player
            GlobalHolder.GetComponent<PlayerStatHandler>().currentBody.GetComponent<CarParts>().speedIncrease +
            GlobalHolder.GetComponent<PlayerStatHandler>().currentWheels.GetComponent<CarParts>().speedIncrease +
            GlobalHolder.GetComponent<PlayerStatHandler>().currentSpoiler.GetComponent<CarParts>().speedIncrease;
        float totalAccelerationMax = //Sets the totalAccelerationMax to the current Acceleration values assigned to each of the car parts currently assigned to the player
            GlobalHolder.GetComponent<PlayerStatHandler>().currentBody.GetComponent<CarParts>().accelerationIncrease +
            GlobalHolder.GetComponent<PlayerStatHandler>().currentWheels.GetComponent<CarParts>().accelerationIncrease +
            GlobalHolder.GetComponent<PlayerStatHandler>().currentSpoiler.GetComponent<CarParts>().accelerationIncrease;
        changeStats(totalSpeedMax, totalAccelerationMax, totalSpeedMax); //Calls method to change the car's stats.
    }


    /*
     *  We use FixedUpdated as that is defined in the editor and if better for physics and will not be susceptible to frame rate drops or increases as this is where all of
     *  the car physics calculations are processed calling different methods from the script to handle different aspects of the cars movement.
    */
    private void FixedUpdate() {
        if (tag == "Player")
        {
            InputHandler();
            if ((frontLeftWheelTransform == null || frontRightWheelTransform == null || backLeftWheelTransform == null || backRightWheelTransform == null))
            {
                setUpWheelTransform(GameObject.FindGameObjectWithTag("Wheels"));
            }
            if (Input.GetKeyDown(KeyCode.R) && (currentScene.name.Contains("RaceScene")))
            {
                resetCar();
            }

        }
        AccelerationHandler();
        TurnHandler();
        WheelRotationHandler();

    }

    /*
     * The InputHandler() method sets the car's movement based on the directional keys the user is pressing each ranging from a scale of -1 - 1 depening on how long the key is pressed
     * and when it is released.
   */
    private void InputHandler()
    {
        forwardAmount = -Input.GetAxis("Vertical");
        turnAmount = Input.GetAxis("Horizontal");
    }

    /*
     * The AccelerationHandler() method applies any vertical input to the users car, the forwardAmount being the vertical input, setting up if statements for if the keys are being pressed
     * when they are released, slowing down the motion of the car when the key is released, keeping the speed with a certain range using the Mathf library Clamp function and then
     * applying the speed and forward force to the velocity.
   */
    protected void AccelerationHandler()
    {
        if (forwardAmount > 0)
        {
            speed += forwardAmount * acceleration * Time.deltaTime;
        }
        if (forwardAmount < 0)
        {
            if (speed > 0)
            {
                speed += forwardAmount * brakeSpeed * Time.deltaTime;
            }
            else
            {
                speed += forwardAmount * reverseSpeed * Time.deltaTime;
            }
        }
        if (forwardAmount == 0)
        {
            if (speed == 0)
            {
                carRigidbody.velocity = Vector3.zero;
            }
            if (speed > 0)
            {
                speed -= idleSlowdown * Time.deltaTime;
            }
            if (speed < 0)
            {

                speed += idleSlowdown * Time.deltaTime;
            }
            if (speed > -0.2 && speed < 0.2)
            {
                speed = 0;
            }
        }


        speed = Mathf.Clamp(speed, speedMin, speedMax);

        carRigidbody.velocity = transform.forward * speed;
        
        carRigidbody.AddForce(Vector3.down * (float)100 * 10);

    }

    /*
     * The TurnHandler() method applies any horizontal input to the users car, the turnAmount being that horizontal input, setting up if statements for if the keys are being pressed
     * when they are released, stopping rotation when the key is released.
   */
    protected void TurnHandler()
    {
        if (speed < 0)
        {
            turnAmount = turnAmount * -1f;
        }

        if (turnAmount > 0 || turnAmount < 0)
        {
            if ((turnSpeed > 0 && turnAmount < 0) || (turnSpeed < 0 && turnAmount > 0))
            {
                float minTurnAmount = 20f;
                turnSpeed = turnAmount * minTurnAmount;
            }
            turnSpeed += turnAmount * turnSpeedAcceleration * Time.deltaTime;
        }
        else
        {
            if (turnSpeed > 0)
            {
                turnSpeed -= turnIdleSlowdown * Time.deltaTime;
            }
            if (turnSpeed < 0)
            {
                turnSpeed += turnIdleSlowdown * Time.deltaTime;
            }
            if (turnSpeed > -1f && turnSpeed < +1f)
            {
                turnSpeed = 0f;
            }
        }

        float speedNormalized = speed / speedMax;
        float invertSpeedNormalized = Mathf.Clamp(1 - speedNormalized, .75f, 1f);

        turnSpeed = Mathf.Clamp(turnSpeed, -turnSpeedMax, turnSpeedMax);

        carRigidbody.angularVelocity = new Vector3(0, turnSpeed * (invertSpeedNormalized * 1f) * Mathf.Deg2Rad, 0);

        if (transform.eulerAngles.x > 2 || transform.eulerAngles.x < -2 || transform.eulerAngles.z > 2 || transform.eulerAngles.z < -2)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }

    /*
     * SetEnemyInputs() method will take two parameters from the EnemyCarDrive script which will dictate the curret forward force and turning force of the enemy car as both player and enemy
     * use this script to control movement.
   */
    public void SetEnemyInputs(float forward, float turn) {
        forwardAmount = forward;
        turnAmount = turn;
    }

    /*
     * SetSpeedMax() is a setter method for speedMax
   */
    public void SetSpeedMax(float speedMax) {
        this.speedMax = speedMax;
    }

    /*
     * SetTurnSpeedMax() is a setter method for turnSpeedMax
   */
    public void SetTurnSpeedMax(float turnSpeedMax) {
        this.turnSpeedMax = turnSpeedMax;
    }
    /*
     * SetTurnSpeedAcceleration() is a setter method for turnSpeedAcceleration
   */
    public void SetTurnSpeedAcceleration(float turnSpeedAcceleration) {
        this.turnSpeedAcceleration = turnSpeedAcceleration;
    }

    /*
     * StopCompletely() with set both the speed and turnSpeed of the car to 0 effectively stopping the car where it is. 
   */
    public void StopCompletely() {
        speed = 0f;
        turnSpeed = 0f;
    }

    /*
     * resetCar() is a method that stops the car's movement and finds the nearest checkpoint while in a race and sets you back at that checkpoints position.
   */
    private void resetCar()
    {
        StopCompletely();
        int currentCheckpoint = GlobalHolder.GetComponent<RaceManager>().getCurrentCheckpoint();
        transform.position = GlobalHolder.GetComponent<RaceManager>().getCurrentCheckpointPos(currentCheckpoint);
        transform.rotation = GlobalHolder.GetComponent<RaceManager>().getCurrentCheckpointRot(currentCheckpoint);
    }

    /*
     * WheelRotationHandler() is a setter method for each of the wheel colliders and wheel transforms so that they rotate when an input is pressed.
   */
    protected void WheelRotationHandler()
    {
        WheelRotation(frontLeftWheelCollider, frontLeftWheelTransform);
        WheelRotation(frontRightWheelCollider, frontRightWheelTransform);
        WheelRotation(backLeftWheelCollider, backLeftWheelTransform);
        WheelRotation(backRightWheelCollider, backRightWheelTransform);
    }

    /*
     * WheelRotation() is a setter method for  the wheel colliders and wheel transforms so that they rotate when an input is pressed.
   */
    private void WheelRotation(WheelCollider col, Transform trans)
    {
        Vector3 currentPosition;
        Quaternion currentRotation;
        col.GetWorldPose(out currentPosition, out currentRotation);
        trans.rotation = currentRotation;
        trans.position = currentPosition;
    }

    /*
    * setUpWheelTransform() is a setter method for getting current the wheel transforms in a scene.
  */
    public void setUpWheelTransform(GameObject currentWheels)
    {
        frontLeftWheelTransform = currentWheels.transform.GetChild(0);
        frontRightWheelTransform = currentWheels.transform.GetChild(1);
        backLeftWheelTransform = currentWheels.transform.GetChild(2);
        backRightWheelTransform = currentWheels.transform.GetChild(3);
    }

    /*
    * changeStats() is a setter method for each of the cars main stats which can be influenced by the dificulty of the race for enemies or the stats or each carpart in the case of the player.
  */
    public void changeStats(float speedMax, float acceleration, float reverseSpeed)
    {
        this.speedMax = speedMax;
        this.acceleration = acceleration;
        this.reverseSpeed = reverseSpeed;
        this.speedMin = -speedMax;
    }

}
