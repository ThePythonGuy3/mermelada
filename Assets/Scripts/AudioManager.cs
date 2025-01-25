using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    
    public static AudioManager instance;

    public AudioMixer musicMixer;

    private AudioSource mAudioSource;
    public AudioClip click;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        mAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadVolume();
    }

    private void LoadVolume()
    {
        float musicVolume = PlayerPrefs.HasKey("Music") ? PlayerPrefs.GetFloat("Music") : 1f;
        musicMixer.SetFloat("Volume", Mathf.Log10(musicVolume) * 20);
    
    }

    public void SetMusic(float volume)
    {
        volume = Mathf.Clamp(volume, 0.001f, 1f);
        musicMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Music", volume);
        PlayerPrefs.Save();
    }
    
    public void PlayClick()
    {
        mAudioSource.PlayOneShot(click);
    }
}
