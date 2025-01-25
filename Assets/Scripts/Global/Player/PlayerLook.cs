using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    /**
     * TODO
     */
    private Camera mainCamera;

    [SerializeField] private Transform pointerTransform;
    [SerializeField] private Transform gunTransform;

    [SerializeField] private float pointerTransformMultiplier = 2.0f;
    private Vector2 newDirection;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (IsUsingGamepad())
        {
            newDirection = AimWithJoystick();
            
        }
        else
        {
            newDirection = AimWithMouse();
        }

        RotateGun(newDirection);
    }

    private Vector2 AimWithMouse()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        Vector2 direction = (worldMousePosition - gunTransform.position).normalized;

        pointerTransform.localPosition = direction.normalized * pointerTransformMultiplier;

        return direction;
    }

    private Vector2 AimWithJoystick()
    {
        Vector2 direction = Gamepad.current.rightStick.ReadValue();

        // Update Pointer
        pointerTransform.localPosition = direction.normalized * pointerTransformMultiplier;

        return direction;
    }

    private void RotateGun(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    #region HELPER
    private bool IsUsingGamepad()
    {
        return Gamepad.current != null;
    }
    #endregion
}