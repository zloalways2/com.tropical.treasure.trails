using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioEngineManager : MonoBehaviour
{
    public static AudioEngineManager singletonReference;

    [FormerlySerializedAs("audioMixer")] [SerializeField] private AudioMixer audioControlMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private void Awake()
    {
        if (singletonReference == null)
        {
            singletonReference = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey(GlobalConfigManager.MUSIC_VOLUME))
        {
            PlayerPrefs.SetFloat(GlobalConfigManager.MUSIC_VOLUME, 0f);
        }
        if (!PlayerPrefs.HasKey(GlobalConfigManager.SOUND_VOLUME))
        {
            PlayerPrefs.SetFloat(GlobalConfigManager.SOUND_VOLUME, 0f);
        }
        
        float bgMusicVolume = PlayerPrefs.GetFloat(GlobalConfigManager.MUSIC_VOLUME);
        float masterVolume = PlayerPrefs.GetFloat(GlobalConfigManager.SOUND_VOLUME);
        
        audioControlMixer.SetFloat(GlobalConfigManager.MUSIC_VOLUME, bgMusicVolume);
        audioControlMixer.SetFloat(GlobalConfigManager.SOUND_VOLUME, masterVolume);
        
        musicSlider.value = bgMusicVolume;
        soundSlider.value = masterVolume;
        
        musicSlider.onValueChanged.AddListener(AdjustMusicVolume);
        soundSlider.onValueChanged.AddListener(AdjustSoundLevel);
    }

    public void AdjustMusicVolume(float volume)
    {
        audioControlMixer.SetFloat(GlobalConfigManager.MUSIC_VOLUME, volume);
        PlayerPrefs.SetFloat(GlobalConfigManager.MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void AdjustSoundLevel(float volume)
    {
        audioControlMixer.SetFloat(GlobalConfigManager.SOUND_VOLUME, volume);
        PlayerPrefs.SetFloat(GlobalConfigManager.SOUND_VOLUME, volume);
        PlayerPrefs.Save();
    }
}