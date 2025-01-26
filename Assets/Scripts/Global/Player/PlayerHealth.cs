using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxTimeHealth = 120;
    private float _currentTimeHealth;

    [SerializeField] private Slider _slider;

    [SerializeField] private TMP_Text _currentTimeHealthText;
    [SerializeField] private TMP_Text _timeHealthBarReactionstText;
    private float lasTimeTextChanged = 0;
    private float maxTimeATExtCanBeShown = 5;

    private Player _playerScript;  // Referencia al script Player

    private void Start()
    {
        SetMaxTimeHealth(_maxTimeHealth);

        _currentTimeHealth = _maxTimeHealth;
        SetTimeHealth(_currentTimeHealth);

        _playerScript = GetComponent<Player>();  // Asignamos la referencia al script Player
    }

    private void FixedUpdate()
    {
        _currentTimeHealth -= Time.deltaTime;
        SetTimeHealth(_currentTimeHealth);

        // Llamar a la función Die si el tiempo llega a cero
        if (_currentTimeHealth <= 0)
        {
            if (_playerScript != null)
            {
                _playerScript.Die();  // Llama a la función Die del script Player
            }
            _currentTimeHealth = 0; // Asegúrate de que no se pase de cero
        }

        if (lasTimeTextChanged > maxTimeATExtCanBeShown)
        {
            _timeHealthBarReactionstText.text = "";
        }
        else
        {
            lasTimeTextChanged += Time.deltaTime;
        }

        WriteCurrentTimeHealth();
    }

    #region MANAGE HEALTH
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
            _currentTimeHealth = 0;
        }

        SetTimeHealth(_currentTimeHealth);
        WriteTextHealth(timeHealthToAdd);
    }
    #endregion

    #region MANAGE TEXT
    private void WriteTextHealth(float timeHealth)
    {
        if (timeHealth < 0)
        {
            _timeHealthBarReactionstText.color = Color.red;
        }
        else
        {
            _timeHealthBarReactionstText.color = Color.green;
        }

        _timeHealthBarReactionstText.text = timeHealth.ToString();

        lasTimeTextChanged = 0;
    }

    private void WriteCurrentTimeHealth()
    {
        int integerPart = (int)_currentTimeHealth;
        _currentTimeHealthText.text = integerPart.ToString("D3");
    }
    #endregion

    private void SetTimeHealth(float health)
    {
        _slider.value = health;
    }

    public void TakeTimeDamage(float timeDamage)
    {
        AddToTimeHealth(- timeDamage);
    }

    public void EnemyTakeDamage()
    {
        Debug.Log("Defeated");
    }
}
