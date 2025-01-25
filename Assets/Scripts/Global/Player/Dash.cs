using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private float _dashDistance;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCouldwon;
    [SerializeField] private AnimationCurve _dashProgressCurve;
    private float _currentTime;

    private PlayerLook _playerLook;

    private void Awake()
    {
        _playerLook = GetComponent<PlayerLook>();
    }

    private void Update()
    {
        if (CanDash() && Input.GetKeyDown(KeyCode.LeftShift))
        {
            float rotationZ = _playerLook.GetMouseAngle();
            Quaternion rotation = Quaternion.Euler(0, 0, rotationZ);
            Vector3 target = Helpers.CalculateTargetPosition(transform.position, rotation, _dashDistance);

            //if (hitToTest.collider.bounds.Contains(telePosition))
            StartCoroutine(Helpers.LerpComplexPosition(transform, transform.position, target, _dashDuration, _dashProgressCurve));

            _currentTime = 0;
        }

        _currentTime = Time.time;
    }

    private bool CanDash()
    {
        return _currentTime > _dashCouldwon;
    }
}
