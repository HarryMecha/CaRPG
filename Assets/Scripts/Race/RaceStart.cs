using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStart : MonoBehaviour
{
    /*
     * Used as an event in the animator to callRaceStart method held in the RaceManager script.
     */

   public void callRaceStart()
    {
        GameObject.Find("GlobalHolder").GetComponent<RaceManager>().RaceStart();
    }

}
