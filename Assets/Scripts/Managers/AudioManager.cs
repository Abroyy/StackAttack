using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background Music")]
    public AudioSource musicSource;
    public AudioClip defaultBGM;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("SFX")]
    public AudioSource sfxSource;
    [Range(0f, 1f)] public float sfxVolume = 0.7f;

    [Header("Projectile SFX")]
    public AudioClip defaultShootSFX;
    public AudioClip uziShootSFX;
    public AudioClip rocketShootSFX;
    public AudioClip boomerangShootSFX;

    [Header("Other SFX")]
    public AudioClip frenzyPickupSFX;
    public AudioClip buttonClickSFX;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = musicVolume;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.volume = sfxVolume;
        }

        PlayMusic(defaultBGM);
    }

    #region Music
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
    }
    #endregion

    #region SFX
    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volumeScale * sfxVolume);
    }

    public void PlayProjectileSFX(ProjectileType type)
    {
        switch (type)
        {
            case ProjectileType.Default: PlaySFX(defaultShootSFX); break;
            case ProjectileType.Uzi: PlaySFX(uziShootSFX); break;
            case ProjectileType.Rocket: PlaySFX(rocketShootSFX); break;
            case ProjectileType.Boomerang: PlaySFX(boomerangShootSFX); break;
        }
    }

    public void PlayFrenzyPickup() => PlaySFX(frenzyPickupSFX);
    public void PlayButtonClick() => PlaySFX(buttonClickSFX);
    #endregion
}
