using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Fields
    public Vector3 offset;
    public Transform carPosition;
    public float translateSpeed;
    public float rotationSpeed;
    #endregion

    /*
	This uses FixedUpdated as that is defined in the editor and if better for physics and will not be susceptible to frame rate drops or increases,
        the code calculates the camera's intended distance from the car at any time and then applies an offset so it is constantly behind it, then
        applies Lerp to it so that in repsonse to any movement made by the player object, the camera will smoothly adjust to match that change.
    */
    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3();
        targetPosition.x = carPosition.TransformPoint(-offset).x;
        targetPosition.y = carPosition.TransformPoint(offset).y;
        targetPosition.z = carPosition.TransformPoint(-offset).z;
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
        Vector3 direction = carPosition.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
