using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Data.Common;
using UnityEngine.UIElements;
using System.IO;


public class DatabaseConfig : MonoBehaviour
{

    /*
     * The DatabaseConfig class is used whenever the database needs to be accessed within the script, the database it set up with a SaveStates table which contains a userID field which acts
     * as a primary key for the rest of the tables in the database the Stats table, which contains all of the players stats that need to be saved e.g. best time in the time trial, money obtained,
     * their position in the overworld when they last saved etc. and the CarParts table which works like an inventory system inserting a new enrty containing the car parts that have been purchased
     * and the user it belongs to.
     */

    #region Fields
    private static string connectionString; //This is a string containing the file location.
    private static string[] statList = new string[11];
    #endregion

    void Start()
    {
        connectionString = "URI=file:" + Application.dataPath + "/Database/CarRPGDatabase.db"; // This is setting the connection string to the location of the database. Application.dataPath is /Assets, the default for unity
        statList[0] = "positionX";
        statList[1] = "positionY";
        statList[2] = "positionZ";
        statList[3] = "rotationX";
        statList[4] = "rotationY";
        statList[5] = "rotationZ";
        statList[6] = "money";
        statList[7] = "bestTime";
        statList[8] = "currentCarBody";
        statList[9] = "currentCarWheels";
        statList[10] = "currentCarSpoiler";

    }

    /*
     * The NewGame method is used when the player wants to make a new game, it opens up a connection string with the database, then executes a insert statement with set base values for each of the
     * different tables in the database SaveStates, Stats and CarParts.
    */
    public static void NewGame(int UserID)
    {
        Debug.Log("NewGame");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) //This opens up the connection to the database using the file location previously set.
        {
            dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
            {
                string sqlQuery = String.Format("INSERT INTO SaveStates(userID) VALUES(\"{0}\")", UserID); //This assigns a new variable sqlQuery to the SQL query we want to perform. I have set to Insert the input email and hashedpassword into the database in the Users table.
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                dbCmd.ExecuteScalar(); //This executes the query.
                 
            }

            using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
            {
                string sqlQuery = "INSERT INTO Stats (userID, positionX, positionY, positionZ, rotationX, rotationY, rotationZ, money, bestTime, currentCarBody, currentCarWheels, currentCarSpoiler) SELECT SaveStates.userID, 40.72, -0.5 , 5.23 , 0 , -90 , 0, 0 , 0.00, 'RustyCarBody', 'RustyCarWheels', 'EmptySpoiler' FROM SaveStates WHERE UserID =  "+ (UserID); //This assigns a new variable sqlQuery to the SQL query we want to perform. This inserts a new record into the stats table with the CharacterStats with corresponding names.
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                dbCmd.ExecuteScalar();
            } 
            

            using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
            {
                string sqlQuery = "INSERT INTO CarParts (name, isLocked, userID) VALUES ('RustyCarBody', 0 , " + (UserID)+")"; //This assigns a new variable sqlQuery to the SQL query we want to perform. This inserts a new record into the stats table with the CharacterStats with corresponding names.
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                dbCmd.ExecuteScalar();
                 sqlQuery = "INSERT INTO CarParts (name, isLocked, userID)  VALUES ('RustyCarWheels', 0, " + (UserID) + ")"; //This assigns a new variable sqlQuery to the SQL query we want to perform. This inserts a new record into the stats table with the CharacterStats with corresponding names.
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                dbCmd.ExecuteScalar();
                sqlQuery = "INSERT INTO CarParts (name, isLocked, userID)  VALUES ('EmptySpoiler', 0, " + (UserID) + ")"; //This assigns a new variable sqlQuery to the SQL query we want to perform. This inserts a new record into the stats table with the CharacterStats with corresponding names.
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                dbCmd.ExecuteScalar();


            }
            dbConnection.Close();
        }
    }

    /*
     * The SlotChecker() method will take a userID as a parameter defined by the slotID in the startMenu, and return whether or not an entry with that userID exists in the database already. 
    */
    public bool SlotChecker(int UserID)
    {
        bool RecordExists = false;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) 
        {
            dbConnection.Open(); 
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) 
            {
                string sqlQuery = "SELECT userID FROM SaveStates WHERE userID =" + (UserID);  
                dbCmd.CommandText = sqlQuery; 
                using (IDataReader reader = dbCmd.ExecuteReader()) 
                {
                    if (reader.IsDBNull(0) == true) 
                    {
                       RecordExists = false;
                    }
                    else
                    {
                        RecordExists = true; 
                    }
                    dbConnection.Close(); 
                    reader.Close(); 
                }
          
            }

            return RecordExists;
        }
    }

    /*
     * The StatUpdater() method takes every variable within the PlayerStatHolder class that needs to be saved for the user to retreive when logging back into the game, it will first run a check to
     * see if that userID already has an entry in the database and if it does it will update the entry in the Stats table with the corresponding userID with the variables in the parameter.
     */
    public static void StatUpdater(int UserID, Vector3 position, Vector3 rotation, double money, double time, string currentCarBody, string currentCarWheels, string currentCarSpoiler) 
    {
        bool RecordExists = false;
        Debug.Log("StatUpdater");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) 
        {
            dbConnection.Open(); 
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) 
            {
                string sqlQuery = "SELECT userID FROM Stats WHERE userID =" + (UserID);  
                dbCmd.CommandText = sqlQuery; 
                using (IDataReader reader = dbCmd.ExecuteReader()) 
                {
                    if (reader.IsDBNull(0) == true) 
                    {
                        RecordExists = false; 
                    }
                    else
                    {
                        RecordExists = true; 
                    }
                    dbConnection.Close(); 
                    reader.Close(); 
                }
            }
            if (RecordExists == true) 
            {
                dbConnection.Open(); 
                using (IDbCommand dbCmd = dbConnection.CreateCommand()) 
                {
                    string sqlQuery = "UPDATE Stats SET positionX = " + (position.x) + ", positionY = " + (position.y) + ", positionZ = " + (position.z) + ", rotationX = " + (rotation.x) + ", rotationY = " + (rotation.y) + ", rotationZ = " + (rotation.z) + ", money = " + (money) + ", bestTime = " + (time)+ ", currentCarBody = '" + (currentCarBody)+ "', currentCarWheels = '" + (currentCarWheels) + "', currentCarSpoiler = '" + (currentCarSpoiler) + "' WHERE userID = " + (UserID); 
                    dbCmd.CommandText = sqlQuery; 
                    dbCmd.ExecuteScalar();
                    dbConnection.Close();
                }
            }
        }
        Debug.Log("Saved");
    }

    /*
     * The unlockedPartsUpdater() method uses userID and an arrayList of unlockedPart names, it will then iterate through the ArrayList and execute an insert statement inserting a new entry
     * with the unlocked car part linked to the userID via a foreign key.
     */
    public static void unlockedPartsUpdater(int UserID, ArrayList unlockedParts) 
    {
        bool RecordExists = false;
        Debug.Log("unlockedPartsUpdater");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) 
        {
            dbConnection.Open(); 
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) 
            {
                string sqlQuery = "SELECT userID FROM CarParts WHERE userID =" + (UserID);  
                dbCmd.CommandText = sqlQuery; 
                using (IDataReader reader = dbCmd.ExecuteReader()) 
                {
                    if (reader.IsDBNull(0) == true) 
                    {
                        RecordExists = false; 
                    }
                    else
                    {
                        RecordExists = true; 
                    }
                    dbConnection.Close(); 
                    reader.Close(); 
                }
            }
            if (RecordExists == true) 
            {
                dbConnection.Open(); 
                using (IDbCommand dbCmd = dbConnection.CreateCommand()) 
                {
                    foreach (string part in unlockedParts)
                    {
                        string sqlQuery = "INSERT INTO CarParts (name, isLocked, userID) VALUES ('"+(part)+"' , 0, "+(UserID)+")";
                        dbCmd.CommandText = sqlQuery; 
                        dbCmd.ExecuteScalar(); 
                    }
                    dbConnection.Close(); 

                }
            }
        }
        Debug.Log("Saved");
    }

    /*
    * The SlotChecker() method will take a userID and partName as parameters defined by the and return whether or not an entry with that partName is found within the CarParts table with 
    * the userID as a foreign key.
   */
    public static Boolean unlockedPartsChecker(int UserID, string partName) 
    { 
        bool isLocked = true;
        bool RecordExists = false;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) 
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) 
            {
                string sqlQuery = "SELECT userID FROM CarParts WHERE userID = " + (UserID);  
                dbCmd.CommandText = sqlQuery; 
                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    if (reader.IsDBNull(0) == true) 
                    {
                        RecordExists = false; 
                    }
                    else
                    {
                        RecordExists = true; 
                    }
                    reader.Close(); 
                }
            }
           
            if (RecordExists == true) 
            {
                using (IDbCommand dbCmd = dbConnection.CreateCommand()) 
                {
                    
                    string sqlQuery = "SELECT name FROM CarParts WHERE name = '"+(partName)+"' AND userID = "+(UserID);  
                    dbCmd.CommandText = sqlQuery; 
                    using (IDataReader reader = dbCmd.ExecuteReader()) 
                    {
                        if (reader.IsDBNull(0) == true) 
                        {
                            isLocked = true; 
                        }
                        else
                        {
                            isLocked = false; 
                        }
                        dbConnection.Close(); 
                        reader.Close(); 
                    }
                }
            }
            return (isLocked);
        }
    }

    /*
     * The StatLoader() method takes a userID as a variable and executes a select statement to get every value held in the stats table using an array of collumn header, then assigning each
     * variable within the PlayerStatHolder class that needs to be retreived when logging back into the game.
     */
    public static void StatLoader(int UserID)  
    {
        GameObject GlobalHolder = GameObject.Find("GlobalHolder"); 
        bool RecordExists = false; 
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) 
        {
            dbConnection.Open(); 
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) 
            {
                string sqlQuery = "SELECT UserID FROM Stats WHERE UserID =" + (UserID); 
                dbCmd.CommandText = sqlQuery; 
                using (IDataReader reader = dbCmd.ExecuteReader()) 
                {
                    if (reader.IsDBNull(0) == true) 
                    {
                        RecordExists = false; 
                    }
                    else
                    {
                        RecordExists = true; 
                    }
                    dbConnection.Close(); 
                    reader.Close(); 
                }
            }
        }
        if (RecordExists == true) 
        {
            using (IDbConnection dbConnection = new SqliteConnection(connectionString))       
            {
                for (int i = 0; i < statList.Length; i++) 
                {
                    dbConnection.Open(); 
                    using (IDbCommand dbCmd = dbConnection.CreateCommand()) 
                    {
                        string sqlQuery = "SELECT " + (statList[i]) + " FROM Stats WHERE userID =" + (UserID); 
                        dbCmd.CommandText = sqlQuery; 
                        using (IDataReader reader = dbCmd.ExecuteReader()) 
                        {
                            while (reader.Read())
                            {
                                if (i == 0)
                                {
                                    GlobalHolder.GetComponent<PlayerStatHandler>().position.x = reader.GetFloat(0);
                                }
                                if (i == 1)
                                {
                                    GlobalHolder.GetComponent<PlayerStatHandler>().position.y = reader.GetFloat(0); 
                                }
                                if (i == 2)
                                {
                                    GlobalHolder.GetComponent<PlayerStatHandler>().position.z = reader.GetFloat(0); 
                                }
                                if (i == 3)
                                {
                                    GlobalHolder.GetComponent<PlayerStatHandler>().rotation.x = reader.GetFloat(0); 
                                }
                                if (i == 4)
                                {
                                    GlobalHolder.GetComponent<PlayerStatHandler>().rotation.y = reader.GetFloat(0); 
                                }
                                if (i == 5)
                                {
                                    GlobalHolder.GetComponent<PlayerStatHandler>().rotation.z = reader.GetFloat(0); 
                                }
                                if (i == 6)
                                {
                                    GlobalHolder.GetComponent<PlayerStatHandler>().money = reader.GetInt32(0); 
                                }
                                if (i == 7)
                                {
                                    GlobalHolder.GetComponent<PlayerStatHandler>().bestTime = reader.GetFloat(0); 
                                }
                                if(i == 8)
                                {
                                  GlobalHolder.GetComponent<PlayerStatHandler>().setCurrentBody(Resources.Load("Prefabs/CarParts/"+reader.GetString(0)) as GameObject);
                                }
                                if(i == 9)
                                {
                                    GlobalHolder.GetComponent<PlayerStatHandler>().setCurrentWheels(Resources.Load("Prefabs/CarParts/" + reader.GetString(0)) as GameObject);
                                }
                                if (i == 10)
                                {
                                    GlobalHolder.GetComponent<PlayerStatHandler>().setCurrentSpoiler(Resources.Load("Prefabs/CarParts/" + reader.GetString(0)) as GameObject);
                                }
                            }
                            dbConnection.Close(); 
                            reader.Close(); 
                        }
                    }
                }
            }
        }
    }


}
    