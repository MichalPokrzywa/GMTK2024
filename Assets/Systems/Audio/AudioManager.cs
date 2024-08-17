using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    [Header ("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Mixer")]
    public AudioStorage audioStorage;

    [Space]

    public float menuMusicVolume;
    public float gameplayMusicVolume;

    public static bool SoundsSettingsOn
    {
        get => DataManager.LoadData<int>("SoundSettings") == 1;
        set { DataManager.SaveData("SoundSettings", value ? 1 : 0); }
    }

    public static bool MusicSettingsOn
    {
        get => DataManager.LoadData<int>("MusicSettings") == 1;
        set { DataManager.SaveData("MusicSettings", value ? 1 : 0); }
    }

    [Header("Audio Source")]
    public AudioSource musicAudioSource;
    public bool IsMusicPlaying => musicAudioSource.isPlaying;

    public UnityEvent onMusicFinished;

    private void Start()
    {
        PlayMenuMusic();
    }

    public void PlaySound(Sound sound)
    {
        if (!SoundsSettingsOn) return;

        AudioClip clip = audioStorage.GetSoundByType(sound);

        if (clip == null) return;

        AudioSource.PlayClipAtPoint(clip, Vector3.zero);
    }

    public void PlaySound(Sound sound, float volume)
    {
        if (!MusicSettingsOn) return;

        AudioClip clip = audioStorage.GetSoundByType(sound);

        if (clip == null) return;

        AudioSource.PlayClipAtPoint(clip, Vector3.zero, volume);
    }

    public void PlaySound(AudioClip sound)
    {
        if (!SoundsSettingsOn) return;

        AudioSource.PlayClipAtPoint(sound, Vector3.zero);
    }

    public void PlayMenuMusic()
    {
        if (!MusicSettingsOn) return;

        CancelInvoke("OnMusicFinished");

        musicAudioSource.volume = menuMusicVolume;
        musicAudioSource.clip = audioStorage.GetRandomMenuMusic();
        musicAudioSource.Play();

        Invoke("OnMusicFinished", musicAudioSource.clip.length);
    }

    public void PlayGameplayMusic()
    {
        CancelInvoke("OnMusicFinished");

        musicAudioSource.clip = audioStorage.GetRandomGameplayMusic();

        musicAudioSource.Play();

        Invoke("OnMusicFinished", musicAudioSource.clip.length);
    }

    private void OnMusicFinished()
    {
        //if(GameManager.GameplayActive)
        //{
        //    PlayGameplayMusic();
        //}
        //else
        //{
        //    PlayMenuMusic();
        //}
    }

    public void SwitchToMenuMusic()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(musicAudioSource.DOFade(0f, 1f));
        sequence.AppendCallback(PlayMenuMusic);
        sequence.Append(musicAudioSource.DOFade(menuMusicVolume, 1f));
    }

    public void SwitchToGameplayMusic()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(musicAudioSource.DOFade(0f, 1f));
        sequence.AppendCallback(PlayGameplayMusic);
        sequence.Append(musicAudioSource.DOFade(gameplayMusicVolume, 1f));
    }

    public void PauseMusic()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(musicAudioSource.DOFade(0f, 0.2f));
        sequence.AppendCallback(() => { musicAudioSource.Stop(); musicAudioSource.volume = gameplayMusicVolume; });
    }

    private void StopMenuMusic()
    {
        musicAudioSource.DOFade(0f, 1f).OnComplete(() => 
        {
            musicAudioSource.Stop();
            musicAudioSource.volume = menuMusicVolume;
            audioMixer.SetFloat("Music", -80f);
        });
    }

    public void SetMusicActivity()
    {
        musicAudioSource.DOKill();

        if (MusicSettingsOn)
        {
            audioMixer.SetFloat("Music", 0f);
            PlayMenuMusic();
        }
        else
        {
            StopMenuMusic();
        }
    }

    public void SetSoundActivity()
    {
        if (SoundsSettingsOn == true)
        {
            audioMixer.SetFloat("Sound", 0f);
        }
        else
        {
            audioMixer.SetFloat("Sound", -80f);
        }
    }

    public void MuteAudio(bool state)
    {
        audioMixer.SetFloat("Sound", state ? -80f : 0f);
        audioMixer.SetFloat("Music", state ? -80f : 0f);
    }
}