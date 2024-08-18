using UnityEngine;


/// <summary>
/// Script used to make an object spin around an axis.
/// </summary>
public class Rotate : MonoBehaviour
{
    /// <summary> Axis to rotate around </summary> 
    public Vector3 axis;
    /// <summary> Speed in rotations/sec </summary>
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis, speed * 360 * Time.unscaledDeltaTime);
    }
}
