using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum difficulty
{
    NULL,
    EASY,
    MEDIUM,
    HARD
}


public class RaceManager : MonoBehaviour
{
    #region Fields
    private GameObject[] checkpoints;
    public difficulty currentDifficulty;
    public GameObject[] EnemyCars;
    public GameObject player;
    public int money, currentCheckpoint, currentLap, noOfCheckpoints,currentPosition;
    public bool timeTrial;
    public GameObject enemyPrefab, finishedPanel;
    private int noOfLaps;
    public TextMeshProUGUI timerText,LapNumberText,PositionText, PositionNumberText;
    private float secondsCount;
    private int minuteCount;
    private bool raceStart,newRecord,raceFinish;
    #endregion

    void Awake()
    {
        raceStart = false;
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        int noOfCheckpoints = checkpoints.Length;
        EnemyCars = GameObject.FindGameObjectsWithTag("Enemy");
        SceneManager.activeSceneChanged += ChangedActiveScene;
        player = GameObject.FindGameObjectWithTag("Player");
        currentCheckpoint = -1;
        currentLap = 0;
        DontDestroyOnLoad(gameObject);
        newRecord = false;
        raceFinish = false;
        currentPosition = 1;
    }

    /*
     * In the Update function the laps and checkpoints related to the player are updated, this is also where the timer is updated and the checks for if the race has
     * finished are done.
     */
    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("RaceScene"))
        {
            checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
            noOfCheckpoints = checkpoints.Length;

            if (noOfCheckpoints - 1 == currentCheckpoint && noOfCheckpoints > 0)
            {
                currentLap++;
                currentCheckpoint = -1;
                resetCheckpoints();
            }
            if (currentLap != noOfLaps && raceStart)
            {
                UpdateTimer();
                UpdateLaps();
                if (timeTrial == false)
                {
                    UpdatePosition();
                }
            }
            if (currentLap == noOfLaps && raceStart)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<CarDriver>().enabled = false;
                raceFinish = true;
            }
            if (raceFinish)
            {
                FinishedPanelSetup();
                raceStart = false;
            }
            
        }
    }

    /*
     * getCurrentCheckpoint() is a getter method for the current checkpoint the player is at.
     */
    public int getCurrentCheckpoint()
    {
        return currentCheckpoint;
    }

    /*
     * setCurrentCheckpoint() is a setter method for the current checkpoint the player is at increasing it to the new value of the checkpoint passed.
     */
    public void setCurrentCheckpoint(int ID)
    {
        currentCheckpoint = ID;
    }

    /*
     * setNoCheckpoint() is a setter method for the current  number of checkpoints in the scene.
     */
    public void setNoCheckpoint(int number)
    {
        noOfCheckpoints = number;
    }

    /*
     * resetCheckpoints() is a method that goes through each checkpoint in the scene and runs the resetisPassed method which resets them to be passed over again after a new lap.
     */
    private void resetCheckpoints()
    {
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.GetComponent<Checkpoint>().resetisPassed();
        }
    }

    /*
    * FinishedPanelSetup() manages the UI for the finished menu when completing a race, setting up it's elements with the corresponding values, each of the shopItemSlots holds a 
    * which changes depending on the race type selected, also calculating the amount of money you receive after you finish the race which updates in PlayerStatHolder.
    */
    private void FinishedPanelSetup()
    {
        finishedPanel.SetActive(true);
        if (timeTrial)
        {
            finishedPanel.transform.Find("YouPlacedText").GetComponent<TextMeshProUGUI>().enabled = false;
            finishedPanel.transform.Find("PositionText").GetComponent<TextMeshProUGUI>().enabled = false;
            finishedPanel.transform.Find("NewRecordText").GetComponent<TextMeshProUGUI>().enabled = false;
            finishedPanel.transform.Find("CurrentTimeText").GetComponent<TextMeshProUGUI>().text = "Time: " + timerText.text;
            float seconds = gameObject.GetComponent<PlayerStatHandler>().bestTime;
            int minutes = 0;
            if (seconds > 60)
            {
                minutes = (int)seconds / 60;
                seconds = seconds - (60 * minutes);
            }
            finishedPanel.transform.Find("BestTimeText").GetComponent<TextMeshProUGUI>().text = "Best Time: " + minutes.ToString("00") + ":" + seconds.ToString("00.00");
            if (secondsCount + (minuteCount * 60) < seconds || seconds == 0)
            {
                finishedPanel.transform.Find("NewRecordText").GetComponent<TextMeshProUGUI>().enabled = true;
                gameObject.GetComponent<PlayerStatHandler>().bestTime = secondsCount + (minuteCount * 60);
                newRecord = true;
            }
            int goldAmount = 20;
            if (newRecord == true)
            {
                goldAmount += 10;
            }
            GetComponent<PlayerStatHandler>().money += goldAmount;
            finishedPanel.transform.Find("GoldEarnedText").GetComponent<TextMeshProUGUI>().text = "You earned " + (goldAmount) + "g";
        }
        else
        {
            finishedPanel.transform.Find("CurrentTimeText").GetComponent<TextMeshProUGUI>().enabled = false;
            finishedPanel.transform.Find("BestTimeText").GetComponent<TextMeshProUGUI>().enabled = false;
            finishedPanel.transform.Find("NewRecordText").GetComponent<TextMeshProUGUI>().enabled = false;
            finishedPanel.transform.Find("PositionText").GetComponent<TextMeshProUGUI>().text = currentPosition.ToString();
            int goldAmount = 10;
            
            switch (currentDifficulty)
            {
                case (difficulty.EASY):
                    goldAmount += 5;
                    break;
                case (difficulty.MEDIUM):
                    goldAmount = goldAmount * 2;
                    goldAmount += 5;
                    break;
                case (difficulty.HARD):
                    goldAmount = goldAmount * 3;
                    goldAmount += 5;
                    break;

            }
            goldAmount -= (5 * currentPosition);
            GetComponent<PlayerStatHandler>().money += goldAmount;
            finishedPanel.transform.Find("GoldEarnedText").GetComponent<TextMeshProUGUI>().text = "You earned " + (goldAmount) + "g";
        }
        raceFinish = false;
    }

    /*
     * getCurrentCheckpointPos() is a getter method for the position of the current checkpoint the player is at by comparing the checkpoints ID to the current checkpoint the players on.
     */
    public Vector3 getCurrentCheckpointPos(int checkpointID)
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        int currentCheckpoint = -1;
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i].GetComponent<Checkpoint>().getID() == checkpointID)
            {
                currentCheckpoint = i;
                return checkpoints[currentCheckpoint].transform.GetChild(0).transform.position;
            }
        }
        return new Vector3();
    }

    /*
     * getCurrentCheckpointPosEnemy() is a getter method for the position of the current checkpoint the enemy is at by comparing the checkpoints ID to the current checkpoint the players on.
     */
    public Vector3 getCurrentCheckpointPosEnemy(int checkpointID)
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        int currentCheckpoint = -1;
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i].GetComponent<Checkpoint>().getID() == checkpointID)
            {
                return getPointWithinCollider(checkpoints[i].GetComponent<BoxCollider>());
            }
        }
        return new Vector3();
    }

    /*
     * getCurrentCheckpointRot() is a getter method for the rotation of the current checkpoint the player is at by comparing the checkpoints ID to the current checkpoint the players on.
     */
    public Quaternion getCurrentCheckpointRot(int checkpointID)
    {
        int currentCheckpoint = -1;
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i].GetComponent<Checkpoint>().getID() == checkpointID)
            {
                currentCheckpoint = i;
            }
        }

        return checkpoints[currentCheckpoint].transform.GetChild(0).transform.rotation;
    }

    /*
     * setDifficulty() is a setter method for the current difficulty of the race.
     */
    public void setDifficulty(int SetDifficulty)
    {
        currentDifficulty = (difficulty)SetDifficulty;
    }

    /*
     * setTimeTrial() is a setter method for the current race type selected, if it's a time trial it will be true, if not it will be false.
     */
    public void setTimeTrial(bool tT)
    {
        timeTrial = tT;
    }

    /*
     * setEnemies() is a setter method for each of the enemy cars in the scene, randomising their stats in a range based on the difficulty of the race.
     */
    public void setEnemies(GameObject[] enemyCars)
    {

        foreach (GameObject car in EnemyCars)
        {
            RandomiseStats(car);
        }
    }

    /*
     * UpdatePosition() is a method that calculates the current position of the player car, by comparing the distance between the transfom position of the enemies and the player
     * to the nearest checkpoint, if they are the same, if not then it will be be determined by whichever in furthest along the track or has the highter number of laps and then will
     * show visually on the position tracker.
     */
    public void UpdatePosition()
    {
        currentPosition = 1;
        float distanceToCheck = Vector3.Distance(getCurrentCheckpointPos(currentCheckpoint), player.transform.position);
        EnemyCars = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in EnemyCars)
        {
            float distanceToEnemyCheck = Vector3.Distance(getCurrentCheckpointPos(currentCheckpoint), enemy.transform.position);
            if (enemy.GetComponent<EnemyCarDrive>().LapNumber > currentLap)
            {
                currentPosition++;
            }
            if (enemy.GetComponent<EnemyCarDrive>().LapNumber == currentLap)
            {
                if (enemy.GetComponent<EnemyCarDrive>().CheckpointID == currentCheckpoint + 1)
                {
                    if (distanceToCheck < distanceToEnemyCheck)
                    {
                        currentPosition++;
                    }
                }
                else if (enemy.GetComponent<EnemyCarDrive>().CheckpointID > currentCheckpoint + 1)
                {
                    currentPosition++;
                }
            }
            
        }
        PositionNumberText.text = currentPosition.ToString();
    }

    /*
     * RandomiseStats() takes a GameObject, enemy car, and randomises it's states in a range based on the diffculty using a switch case and then sets the stats by running the changeStats method
     * in the car GameObject.
     */
    private void RandomiseStats(GameObject car)
    {
        EnemyCarDrive enemycarDrive = car.GetComponent<EnemyCarDrive>();
        switch (currentDifficulty)
        {
            case (difficulty.EASY):
                noOfLaps = 1;
                float randomSpeed =  Random.Range(18, 22);
               float randomAcceleration = Random.Range(18, 22);
                enemycarDrive.changeStats(randomSpeed, randomAcceleration, randomSpeed);
                break;
            case (difficulty.MEDIUM):
                noOfLaps = 2;
                randomSpeed = Random.Range(30, 34);
                Debug.Log(randomSpeed);
                randomAcceleration = Random.Range(30, 34);
                enemycarDrive.changeStats(randomSpeed, randomAcceleration, randomSpeed);
                break;
            case (difficulty.HARD):
                noOfLaps = 3;
                randomSpeed = Random.Range(35, 39);
                randomAcceleration = Random.Range(35, 39);
                enemycarDrive.changeStats(randomSpeed, randomAcceleration, randomSpeed);
                break;

        }


    }

    /*
     * RaceStart() is called when the race starts after the countdown and makes sure that certain UI elements are enabled and that the players and enemies are free to move.
     */
    public void RaceStart()
    {
        raceStart = true;
        player.GetComponent<CarDriver>().enabled = true;
        LapNumberText.enabled = true;
        if (timeTrial)
        {
            
        }
        else
        {
            PositionText.enabled = true;
            PositionNumberText.enabled = true;
            foreach (GameObject Enemy in EnemyCars)
            {
                Enemy.GetComponent<EnemyCarDrive>().enabled = true;
            }
        }
    }

    /*
    * ChangedActiveScene is a function in the UnityEngine.SceneManagement library that runs when a new scene is loaded. This method in this class check that the current scene is race scene and 
    * sets up the all the UI elemets, the player and enemy GameObjects and resets all of the race associated values as they should be at the beginning of the race.
    */
    public void ChangedActiveScene(Scene prevScene, Scene newScene)
    {
        if (newScene.name.Contains("RaceScene"))
        {
            
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<CarDriver>().enabled = false;
            timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
            LapNumberText = GameObject.Find("LapNumberText").GetComponent<TextMeshProUGUI>();
            PositionText = GameObject.Find("PositionText").GetComponent<TextMeshProUGUI>();
            PositionNumberText = GameObject.Find("PositionNumberText").GetComponent<TextMeshProUGUI>();
            finishedPanel = GameObject.Find("FinishedPanel");
            finishedPanel.SetActive(false);
            newRecord = false;
            raceStart = false;
            raceFinish = false;
            currentLap = 0;
            secondsCount = 0;
            minuteCount = 0;
            LapNumberText.enabled = false;
            if (timeTrial)
            {
                noOfLaps = 1;
                PositionText.enabled = false;
                PositionNumberText.enabled = false;
                timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
                LapNumberText = GameObject.Find("LapNumberText").GetComponent<TextMeshProUGUI>();            
            }
            else
            {
                timerText.enabled = false;
                GameObject[] EnemySpawnPositions = GameObject.FindGameObjectsWithTag("EnemySpawn");
                foreach (GameObject position in EnemySpawnPositions)
                {
                    GameObject newEnemy = Instantiate(enemyPrefab, position.transform.position, position.transform.rotation);
                }
                EnemyCars = GameObject.FindGameObjectsWithTag("Enemy");
            }
            EnemyCars = GameObject.FindGameObjectsWithTag("Enemy");
            setEnemies(EnemyCars);
            foreach (GameObject Enemy in EnemyCars)
            {
                Enemy.GetComponent<EnemyCarDrive>().enabled = false;
            }
        }
    }

    /*
     * getPointWithinCollider() takes the collider of the current checkpoint and randomly generates a number within the bounds of that collider and returns it, used to set the enemies
     * target position after passing a checkpoint in order to stop them from following the same path.
     */
    public Vector3 getPointWithinCollider(BoxCollider collider)
    {
        Vector3 bounds = collider.size / 2f;
        Vector3 coordinate = new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y), Random.Range(-bounds.z, bounds.z));
        return collider.transform.TransformPoint(coordinate);

    }

    /*
     * UpdateTimer() updates the timer value in seconds and minutes in accordance with Time.deltaTime and visually represents that through the timer object on the UI.
     */
    public void UpdateTimer()
    {
        secondsCount += Time.deltaTime;
        timerText.text = (minuteCount).ToString("00") + ":" + (secondsCount).ToString("00.00");
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }

    }

    /*
     * UpdateLaps() updates the lap number text when a lap is completed.
     */
    public void UpdateLaps()
    {
        LapNumberText.text = (currentLap + 1) + "/" + (noOfLaps);
    }
}
