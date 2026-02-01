using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;

    [SerializeField] AudioClip[] audioClips;
    AudioSource soundEffectAudio;
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
        soundEffectAudio = GetComponentInChildren<AudioSource>();
    }
    public void PlaySoundByName(string soundName, float volumeScale = 1)
    {
        AudioClip clipToPlay = GetClip(soundName);
        if(clipToPlay != null) 
        {
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

    
}
