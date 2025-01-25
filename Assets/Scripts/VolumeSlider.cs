using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider musicSlider;
  
    private void Start()
    {
        SetSliders();   

        musicSlider.onValueChanged.AddListener((volume) =>
        {
            ChangeMusicVolume(volume);
        }); 
    }
    
    public void ChangeMusicVolume(float volume)
    {
        AudioManager.instance.SetMusic(volume);
    }

    private void SetSliders()
    {
        float musicVolume = PlayerPrefs.HasKey("Music") ? PlayerPrefs.GetFloat("Music") : 1f;

        musicSlider.value = musicVolume;
    }

}
