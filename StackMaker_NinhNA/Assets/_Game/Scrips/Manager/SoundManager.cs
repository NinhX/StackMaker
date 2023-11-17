using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private static AudioSource audioSource;

    public static SoundManager Instance => instance = instance != null ? instance : FindObjectOfType<SoundManager>();

    [SerializeField] private AudioClip clipVictory;
    [SerializeField] private AudioClip clipAddBrick;
    [SerializeField] private AudioClip clipRemoveBrick;
    [SerializeField] private AudioClip clipPlaying;

    public void OnInit()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("volume", 1);
    }

    public void PlayVictoryMusic()
    {
        PlayMusic(clipVictory);
    }

    public void PlayAddBrick()
    {
        PlaySound(clipAddBrick);
    }
    public void PlayRemoveBrick()
    {
        PlaySound(clipRemoveBrick);
    }
    public void PlayPlaying()
    {
        PlayMusic(clipPlaying);
    }
    private void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip || !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SaveVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat("volume", audioSource.volume);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
