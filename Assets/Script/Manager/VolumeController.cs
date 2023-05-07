using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public static VolumeController instance;

    public AudioClip[] backgroundMusic;
    public AudioSource musicSource;
    public AudioSource tempEffectSource;

    public AudioMixer mixer;

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider effectsVolume;

    public const string MIXER_MASTER = "MasterVolume";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        masterVolume.onValueChanged.AddListener(SetMasterVolume);
        musicVolume.onValueChanged.AddListener(SetMusicVolume);
        effectsVolume.onValueChanged.AddListener(SetSFXVolume);

        if (backgroundMusic.Length < 2)
        {
            musicSource.clip = backgroundMusic[0];
            musicSource.Play();
        }
        else
        {
            musicSource.clip = backgroundMusic[1];
            musicSource.PlayOneShot(backgroundMusic[0]);
        }


    }

    private void Start()
    {
        masterVolume.value = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 1f);
        musicVolume.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        effectsVolume.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);

    }

    private void Update()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, masterVolume.value);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicVolume.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, effectsVolume.value);
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat(MIXER_MASTER, Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(volume) * 20);
    }

    public void BackToMenu()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.MENU);
    }

    public void GameLose()
    {
        tempEffectSource.Play();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
