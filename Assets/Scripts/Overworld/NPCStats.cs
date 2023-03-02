using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NPCStats : MonoBehaviour
{
    /*
     * NPCStats is a class that holds all the stats for the NPC and their getter and setter methods.
     */

    #region Fields
    public string npc_name; 
    public string[] firstNpc_dialouge = new string[3];
    public string regular_dialouge; 
    public string[] npc_options = new string[3];
    public string npc_shopDialouge;
    #endregion


    /*
     * getnpc_options() getter method returns all the options for the NPC.
     */
    public string[] getnpc_options()
    {
        return npc_options;
    }
    
    /*
    * getNpc_name() getter method returns name of NPC.
    */
    public string getNpc_name()
    {
        return npc_name;
    }

    /*
    * getNpc_Dialouge() getter method returns the dialouge of the NPC set in inspector, it returns a value based on whether it's your first time interacting with the NPC or not.
    */
    public string getNpc_Dialouge(bool firstTime, int currentPage)
    {
        if (firstTime)
        {
            return firstNpc_dialouge[currentPage];
        }
        else
        {
            return regular_dialouge;
        }
        
    }
    /*
    * getNpc_shopDialouge() getter method returns the shop dialouge of NPC.
    */
    public string getNpc_shopDialouge()
    {
        return npc_shopDialouge;

    }

    /*
    * getNpc_DialougeLength getter method returns length of NPC_Dialouge array.
    */
    public int getNpc_DialougeLength()
    {
        return firstNpc_dialouge.Length;
    }
}