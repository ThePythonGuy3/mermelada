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
        // Verificar si ya existe una instancia del AudioManager
        if (instance == null)
        {
            // Si no existe, asignamos esta instancia como la única
            instance = this;
            DontDestroyOnLoad(gameObject);  // No destruir el objeto entre escenas
        }
        else
        {
            // Si ya existe, destruimos este objeto duplicado
            Destroy(gameObject);
            return;
        }

        // Obtener el componente AudioSource
        mAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadVolume();
    }

    private void LoadVolume()
    {
        // Cargar el volumen de la música desde PlayerPrefs
        float musicVolume = PlayerPrefs.HasKey("Music") ? PlayerPrefs.GetFloat("Music") : 1f;
        musicMixer.SetFloat("Volume", Mathf.Log10(musicVolume) * 20);
    }

    public void SetMusic(float volume)
    {
        // Ajustar el volumen de la música y guardarlo en PlayerPrefs
        volume = Mathf.Clamp(volume, 0.001f, 1f); // Volumen limitado entre 0 y 1
        musicMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Music", volume);
        PlayerPrefs.Save();
    }

    public void PlayClick()
    {
        // Reproducir el sonido de clic
        mAudioSource.PlayOneShot(click);
    }
}
