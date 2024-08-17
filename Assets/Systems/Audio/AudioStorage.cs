using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Storage")]
public class AudioStorage : ScriptableObject
{
    [Header("Sounds")]
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip clickBack;

    [Header("Menu music")]
    [SerializeField] private List<AudioClip> menuMusicClips;

    [Header("Gameplay music")]
    [SerializeField] private List<AudioClip> gameplayMusicClips;

    public AudioClip GetSoundByType(Sound sound)
    {
        switch (sound)
        {
            case Sound.Click:
                return click;

            case Sound.ClickBack:
                return clickBack;
        }

        return null;
    }

    public AudioClip GetRandomMenuMusic()
    {
        List<AudioClip> availableClips = new List<AudioClip>(menuMusicClips);

        if(availableClips.Exists(clip => clip == DependencyManager.audioManager.musicAudioSource.clip))
        {
            availableClips.Remove(DependencyManager.audioManager.musicAudioSource.clip);
        }

        return availableClips[Random.Range(0, availableClips.Count)];
    }

    public AudioClip GetRandomGameplayMusic()
    {
        List<AudioClip> availableClips = new List<AudioClip>(gameplayMusicClips);

        if (availableClips.Exists(clip => clip == DependencyManager.audioManager.musicAudioSource.clip))
        {
            availableClips.Remove(DependencyManager.audioManager.musicAudioSource.clip);
        }

        return availableClips[Random.Range(0, availableClips.Count)];
    }
}

public enum Sound
{
    None = 0,
    Click = 1,
    ClickBack = 2,
    Switch = 3,
}