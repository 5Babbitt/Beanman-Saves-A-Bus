using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public static AudioMixer mixer { get; private set; }

    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambienceSource;

    private static AudioSource staticMusicSource;
    private static AudioSource staticSfxSource;
    private static AudioSource staticAmbienceSource;

    private void Start() 
    {
        staticMusicSource = musicSource;
        staticSfxSource = sfxSource;
        staticAmbienceSource = ambienceSource;
        
        mixer = mainMixer;
    }
    
    public static void PlayMusic(AudioClip clip)
    {
        staticMusicSource.clip = clip;
        staticMusicSource.Play();
    }

    public static void PlayRandomMusic(AudioClip[] clips)
    {
        var clipIndex = Random.Range(0, clips.Length);

        staticMusicSource.clip = clips[clipIndex];
        staticMusicSource.Play();
    }

    public static void PlaySoundEffect(AudioClip clip)
    {
        staticSfxSource.PlayOneShot(clip);
    }

    public static void PlayRandomSoundEffect(AudioClip[] clips)
    {
        var clipIndex = Random.Range(0, clips.Length);

        staticSfxSource.PlayOneShot(clips[clipIndex]);
    }

    public static void PlayAmbience(AudioClip clip)
    {
        staticAmbienceSource.PlayOneShot(clip);
    }

    public static void PlayRandomAmbience(AudioClip[] clips)
    {
        var clipIndex = Random.Range(0, clips.Length);

        staticAmbienceSource.PlayOneShot(clips[clipIndex]);
    }

    public static void StopAmbience()
    {
        if (staticAmbienceSource == null)
            return;
        
        if (staticAmbienceSource.isPlaying)
            staticAmbienceSource.Stop();
    }

    public static void StopAllAudio()
    {
        staticMusicSource.Stop();
        staticAmbienceSource.Stop();
        staticSfxSource.Stop();
    }
}
