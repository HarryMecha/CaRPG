using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class garageSlot : MonoBehaviour
{
    /*
     * Class for any garageSlot.
     */

    #region Fields
    public GameObject[] partArray = new GameObject[2];
    #endregion

    /*
     * getPartArray() getter method, takes the current position in the array of the currentslot and returns the prefab stored at that index number.
     */
    public GameObject getPartArray(int currentslot)
    {
        return partArray[currentslot];
    }

    /*
     * getPartArraySize() getter method, gets the size of the partArray.
     */
    public int getPartArraySize()
    {
        return partArray.Length;
    }

}
