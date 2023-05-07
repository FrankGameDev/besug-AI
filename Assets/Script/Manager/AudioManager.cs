using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer; 

    public static AudioManager _instance;

    public const string MASTER_KEY = "masterVolKey";
    public const string MUSIC_KEY = "musicVolKey";
    public const string SFX_KEY = "sfxVolKey";

    private void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (_instance != this)
        {
            Destroy(_instance.gameObject);
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LoadVolume();
    }

    public void LoadVolume()
    {
        float masterVol = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVol = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVol = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        mixer.SetFloat(VolumeController.MIXER_MASTER, Mathf.Log10(masterVol) * 20);
        mixer.SetFloat(VolumeController.MIXER_MUSIC, Mathf.Log10(musicVol) * 20);
        mixer.SetFloat(VolumeController.MIXER_SFX, Mathf.Log10(sfxVol) * 20);
    }

    private void Start()
    {
        //BackgroundMusicPlayer();
    }

}