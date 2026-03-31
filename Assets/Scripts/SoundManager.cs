using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set; }

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioclip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioclip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioclip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}
