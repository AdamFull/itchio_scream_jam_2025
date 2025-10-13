using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [Range(0f, 1f)]
    [Tooltip("Chance for sound to play when triggered (0 = never, 1 = always)")]
    public float triggerChance = 1f;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= triggerChance)
            {
                audioSource.Play();
            }
        }
    }
}
