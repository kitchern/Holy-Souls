using UnityEngine;

public class RotateCollectible : MonoBehaviour
{
    public float rotationSpeed = 50f; // Rotation speed in degrees per second

    // Update is called once per frame
    void Update()
    {
        // Rotate around the up axis of the GameObject
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
