using UnityEngine;
public class RandomRingRotation : MonoBehaviour
{
    // Array of potential rotations
    private float[] possibleRotations = {0f, 60f, 120f, 180f, 240f, 300f};

    private void Start()
    {
        SetRandomRotation();
    }

    // Set the ring to a random rotation from the possible rotations
    public void SetRandomRotation()
    {
        float randomRotationZ = possibleRotations[Random.Range(0, possibleRotations.Length)];
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, randomRotationZ);
    }
}