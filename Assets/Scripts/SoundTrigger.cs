using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [Range(0f, 1f)]
    [Tooltip("Chance for sound to play when triggered (0 = never, 1 = always)")]
    public float triggerChance = 1f;

    [Header("Sound Settings")]
    [Tooltip("List of sound effects to play. If empty, uses AudioSource clip")]
    public List<AudioClip> soundEffects = new List<AudioClip>();
    [Tooltip("Audio volume play range")]
    [Range(0.1f, 1f)]
    public float volumeRange = 1f;

    [Tooltip("If true, shuffles sounds to prevent consecutive repeats")]
    public bool useShuffle = true;

    private AudioSource audioSource;

    private List<AudioClip> shuffledSounds = new List<AudioClip>();
    private int currentSoundIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (soundEffects.Count > 1 && useShuffle)
        {
            ShuffleSounds();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float randomValue = Random.Range(0f, 1f);

            if (randomValue <= triggerChance)
            {
                PlaySound();
            }
        }
    }

    void PlaySound()
    {
        if (soundEffects.Count == 0)
        {
            audioSource.Play();
            return;
        }

        float effectVolume = Random.Range(0.5f, volumeRange);

        if (soundEffects.Count == 1)
        {
            audioSource.PlayOneShot(soundEffects[0], effectVolume);
            return;
        }

        if (useShuffle)
        {
            // Same algo like for foodsteps
            if (currentSoundIndex >= shuffledSounds.Count)
            {
                ShuffleSounds();
            }

            AudioClip soundClip = shuffledSounds[currentSoundIndex];
            audioSource.PlayOneShot(soundClip, effectVolume);

            currentSoundIndex = (currentSoundIndex + 1) % shuffledSounds.Count;
        }
        else
        {
            int randomIndex = Random.Range(0, soundEffects.Count);
            audioSource.PlayOneShot(soundEffects[randomIndex], effectVolume);
        }
    }

    void ShuffleSounds()
    {
        if (soundEffects.Count == 0)
            return;

        AudioClip lastPlayed = null;
        if (shuffledSounds.Count > 0 && currentSoundIndex > 0)
        {
            lastPlayed = shuffledSounds[currentSoundIndex - 1];
        }

        shuffledSounds = ShuffleUtility.SpotifyShuffle(soundEffects, lastPlayed);

        currentSoundIndex = 0;
    }
}
