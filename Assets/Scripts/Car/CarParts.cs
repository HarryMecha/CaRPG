using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarParts : MonoBehaviour
{
    public int ItemID;
    public Sprite Icon;
    public string partName;
    public int speedIncrease;
    public int accelerationIncrease;
    public int handlingIncrease;
    public bool meshReorientation;
    public GameObject partMesh;
    public int BuyAmount;
    public bool isLocked;
    public string Description;

    public GameObject getPartMesh()
    {
        return partMesh;
    }

    public Sprite getIcon()
    {
        return Icon;
    }

    public bool getIsLocked()
    {
        PlayerStatHandler statHandler = GameObject.Find("GlobalHolder").GetComponent<PlayerStatHandler>();
        if ((DatabaseConfig.unlockedPartsChecker(statHandler.getUserID(), partName) == false))
        {
            setIsLocked(false);
        }
        else if ((statHandler.getNewUnlockedParts().Contains(partName) == true))
        {
            setIsLocked(false);
        }
        else
        {
            setIsLocked(true);
        }
        return isLocked;
    }

    public void setIsLocked(bool anyLocked)
    {
        isLocked = anyLocked; 
    }
}
