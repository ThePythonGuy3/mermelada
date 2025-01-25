using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxTimeHealth = 120;
    private float _currentTimeHealth;

    [SerializeField] private Slider _slider;

    private void Start()
    {
        SetMaxTimeHealth(_maxTimeHealth);

        _currentTimeHealth = _maxTimeHealth;
        SetTimeHealth(_currentTimeHealth);
    }

    private void FixedUpdate()
    {
        _currentTimeHealth -= Time.deltaTime;
        SetTimeHealth(_currentTimeHealth);
    }

    public void AddToMaxTimeHealth(float maxTimeHealthToAdd)
    {
        _maxTimeHealth += maxTimeHealthToAdd;
        SetMaxTimeHealth(_maxTimeHealth);
        AddToTimeHealth(maxTimeHealthToAdd);
    }

    private void SetMaxTimeHealth(float maxHealth)
    {
        _slider.maxValue = maxHealth;
    }

    public void AddToTimeHealth(float timeHealthToAdd)
    {
        float newTimeHealth = _currentTimeHealth + timeHealthToAdd;

        if (newTimeHealth < _maxTimeHealth && newTimeHealth > 0)
        {
            _currentTimeHealth = newTimeHealth;
        }
        else if (newTimeHealth >= _maxTimeHealth)
        {
            _currentTimeHealth = _maxTimeHealth;
        }
        else
        {
            Player.PlayerIsDead();
            _currentTimeHealth = 0;
        }

        SetTimeHealth(_currentTimeHealth);
    }

    private void SetTimeHealth(float health)
    {
        _slider.value = health;
    }

    public void GetTimeDamage(float timeDamage)
    {
        AddToTimeHealth(- timeDamage);
    }

    public void GetDamage()
    {
        Debug.Log("Defeated");
    }
}