using UnityEngine;

public class RotateOnAxis : MonoBehaviour
{

    public Vector3 rotationSpeed;

    void Update()
    {
        transform.Rotate(rotationSpeed);
    }
}
