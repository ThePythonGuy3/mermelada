/*using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Camera mainCamera;
    

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void LookTo(Vector2 _direction)
    {
        Debug.Log(_direction);
        Vector2 position = mainCamera.ScreenToWorldPoint(_direction);
        transform.up = (position - (Vector2)transform.position).normalized;
        Debug.Log(transform.up);
    }
}*/

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private Transform gunTransform;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (IsUsingGamepad())
        {   
            AimWithJoystick();
        }
        else
        {
            AimWithMouse();
        }
    }

    private void AimWithMouse()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        Vector2 aimDirection = (worldMousePosition - gunTransform.position).normalized;
        RotateGun(aimDirection);
    }

    private void AimWithJoystick()
    {
        Vector2 aimDirection = Gamepad.current.rightStick.ReadValue();
        Debug.Log(aimDirection);
        RotateGun(aimDirection);
    }

    private void RotateGun(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    #region HELPER
    private bool IsUsingGamepad()
    {
        return Gamepad.current != null && Gamepad.current.leftStick.ReadValue().magnitude > 0.1f;
    }
    #endregion
}