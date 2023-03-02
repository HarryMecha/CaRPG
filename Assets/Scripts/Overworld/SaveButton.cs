using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    GameObject globalHolder;
    void Start()
    {
        globalHolder = GameObject.Find("GlobalHolder");
    }

    
    public void save()
    {
        globalHolder.GetComponent<PlayerStatHandler>().SaveDB();
    }

}
