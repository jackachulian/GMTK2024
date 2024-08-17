using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputListener : MonoBehaviour {

    private PlayerMovement movement;
    private PlayerBuildManager buildManager;

    private void Awake() {
        movement = GetComponent<PlayerMovement>();
        buildManager = GetComponent<PlayerBuildManager>();
    }

    public void OnMove(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        movement.SetMoveDirection(v.x, v.y);
        
    }

    public void OnCamRotate(InputValue value)
    {
        // Debug.Log("OnMove");
        Vector2 v = value.Get<Vector2>();
        movement.SetCamRotateDirection(new Vector3(v.x, v.y, 0));
    }

    public void OnJump(InputValue value)
    {
        float v = value.Get<float>();

        // 1 when pressed, 0 when not
        if (v != 0f) movement.JumpPress();
    }

    public void OnBuild(InputValue value)
    {
        float v = value.Get<float>();

        if (v != 0f) buildManager.PlaceBuildable();
    }

    public void OnSelectBuildable(InputValue value)
    {
        float v = value.Get<float>();

        if (v != 0f) buildManager.CycleBuildableSelection();
    }
}