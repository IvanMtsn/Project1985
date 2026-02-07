using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource SfxSource;
    public AudioSource MusicSource;
    [SerializeField] AudioSource _audioSourcePrefab;
    void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void PlaySound(AudioClip audio, float volume)
    {
        SfxSource.PlayOneShot(audio, volume);
    }
    public void PlaySoundAtPosition(AudioClip audio, float volume, Vector3 position)
    {
        AudioSource audioSource = Instantiate(_audioSourcePrefab, position, Quaternion.identity);
        audioSource.GetComponent<AudioSource>().PlayOneShot(audio, volume);
        Destroy(audioSource, audio.length + Time.deltaTime);
    }
    public void PlayMusic(AudioClip music, bool loop)
    {
        MusicSource.clip = music;
        MusicSource.loop = loop;
        MusicSource.Play();
    }
    public void StopMusic()
    {
        MusicSource.Stop();
    }
    public void FadeOutSound(AudioSource audioSource, float duration)
    {
        StartCoroutine(FadeOutSoundCoroutine(audioSource, duration));
    }
    IEnumerator FadeOutSoundCoroutine(AudioSource audioSource, float duration)
    {
        float time = 0;
        while(time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, time / duration);
            yield return null;
        }
    }
}
