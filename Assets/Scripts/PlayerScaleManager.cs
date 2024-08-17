using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScaleManager : MonoBehaviour {
    public float currentScale {get; private set;} = 1f;

    public void OnChangeScale(InputValue value)
    {
        float v = value.Get<float>();

        // Debug.Log("Mouse scroll: " + v);
        if (v != 0)
        {
            currentScale = Mathf.Clamp(currentScale + v / 400, LevelData.current.minScale, LevelData.current.maxScale);
        }
    }
}