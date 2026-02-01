using System.Collections;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;

    [SerializeField] AudioClip[] audioClips;
    [SerializeField] AudioSource soundEffectAudio;

    [SerializeField] AudioSource faceMusic;
    [SerializeField] AudioSource votingMusic;
    AudioSource currentAudioSource;

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
    }
    private void Start()
    {
        PlayFaceMusic();
    }
    public void PlaySoundByName(string soundName, float volumeScale = 1, float pitchShift = 0f)
    {
        AudioClip clipToPlay = GetClip(soundName);
        if(clipToPlay != null) 
        {
            soundEffectAudio.pitch = 1f + Random.Range(-pitchShift, pitchShift);
            soundEffectAudio.PlayOneShot(clipToPlay, volumeScale);
        }
        else
        {
            Debug.LogWarning($"Audio clip not found: {soundName}");
        }
    }
    private AudioClip GetClip(string clipName)
    {
        for(int i = 0; i < audioClips.Length; i++)
        {
            if (audioClips[i].name == clipName)
            {
                return audioClips[i];
            }
        }
        return null;
    }

    public void StopMusic()
    {
        if(currentAudioSource == null)
        {
            return;
        }
        StartCoroutine(FadeOutMusic(currentAudioSource));
        currentAudioSource = null;
    }
    IEnumerator FadeOutMusic(AudioSource source)
    {
        float vol = 1f;
        while(vol > 0f)
        {
            vol -= Time.deltaTime;
            if(vol < 0)
            {
                vol = 0f;
            }
            source.volume = vol;
            yield return null;
        }
        source.Stop();
    }
    IEnumerator FadeInMusic(AudioSource source)
    {
        source.Play();
        float vol = 0f;
        while (vol < 1f)
        {
            vol += Time.deltaTime;
            if(vol > 1)
            {
                vol = 1f;
            }
            source.volume = vol;
            yield return null;
        }
    }
    public void PlayFaceMusic()
    {
        if (currentAudioSource == faceMusic && faceMusic.isPlaying)
            return;

        StopMusic();
        currentAudioSource = faceMusic;
        StartCoroutine(FadeInMusic(faceMusic));
    }

    public void PlayVotingMusic()
    {
        StopMusic();
        currentAudioSource = votingMusic;
        StartCoroutine(FadeInMusic(votingMusic));
    }

}
