using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    public Slider master, music, sfx, ambience;

    private void Start() 
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
            LoadVolumes();
        else   
            SetVolumes();
    }

    public void SetMaster()
    {
        float volume = master.value;
        
        AudioManager.mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusic()
    {
        float volume = music.value;
        
        AudioManager.mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFX()
    {
        float volume = sfx.value;
        
        AudioManager.mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetAmbience()
    {
        float volume = ambience.value;
        
        AudioManager.mixer.SetFloat("AmbienceVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("AmbienceVolume", volume);
    }

    public void LoadVolumes()
    {
        master.value = PlayerPrefs.GetFloat("MasterVolume");
        music.value = PlayerPrefs.GetFloat("MusicVolume");
        sfx.value = PlayerPrefs.GetFloat("SFXVolume");
        ambience.value = PlayerPrefs.GetFloat("AmbienceVolume");

        SetVolumes();
    }

    private void SetVolumes()
    {
        SetMaster();
        SetMusic();
        SetSFX();
        SetAmbience();
    }
}
