using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmbientConductor : MonoBehaviour
{
    [System.Serializable]
    public class AudioGroup
    {
        [Header("General")]
        public string groupName = "Audio Group";
        public string mixerChannel = "Master";
        public List<AudioClip> audioClips = new List<AudioClip>();

        [Header("Volume")]
        [Range(0f, 1f)]
        public float minVolume = 0.1f;
        [Range(0f, 1f)]
        public float maxVolume = 1f;

        [Header("Timing")]
        [Tooltip("Minimum time (seconds) between plays")]
        public float minTimeInterval = 5f;
        [Tooltip("Maximum time (seconds) between plays")]
        public float maxTimeInterval = 15f;

        [Header("Playback Options")]
        [Tooltip("Wait for current clip to finish before scheduling next")]
        public bool waitUntilDone = true;
        [Tooltip("Use shuffle algorithm to prevent consecutive repeats")]
        public bool useShuffle = true;
        [Tooltip("Loop individual clips")]
        public bool loop = false;

        [Header("Spatial Audio")]
        [Range(0f, 1f)]
        [Tooltip("0 = 2D sound, 1 = 3D sound")]
        public float spatialBlend = 0f;

        [HideInInspector]
        public AudioSource audioSource;
        [HideInInspector]
        public bool isPlaying = false;

        private List<AudioClip> shuffledClips = new List<AudioClip>();
        private int currentClipIndex = 0;

        public AudioClip GetNextClip()
        {
            if (audioClips.Count == 0)
                return null;

            if (audioClips.Count == 1)
                return audioClips[0];

            if (useShuffle)
            {
                if (currentClipIndex >= shuffledClips.Count)
                {
                    ShuffleClips();
                }

                AudioClip clip = shuffledClips[currentClipIndex];
                currentClipIndex++;
                return clip;
            }
            else
            {
                int randomIndex = Random.Range(0, audioClips.Count);
                return audioClips[randomIndex];
            }
        }

        private void ShuffleClips()
        {
            // All the same as in player and in sound trigger...
            if (audioClips.Count == 0)
                return;

            AudioClip lastPlayed = null;
            if (shuffledClips.Count > 0 && currentClipIndex > 0)
            {
                lastPlayed = shuffledClips[currentClipIndex - 1];
            }

            shuffledClips = ShuffleUtility.SpotifyShuffle(audioClips, lastPlayed);

            currentClipIndex = 0;
        }

        public float GetRandomVolume()
        {
            return Random.Range(minVolume, maxVolume);
        }

        public float GetRandomInterval()
        {
            return Random.Range(minTimeInterval, maxTimeInterval);
        }
    }

    [Header("General")]
    public AudioMixer mixer;
    public List<AudioGroup> audioGroups = new List<AudioGroup>();

    [Header("Conductor Settings")]
    [Tooltip("Start playing ambient sounds on awake")]
    public bool playOnAwake = true;

    private List<Coroutine> activeCoroutines = new List<Coroutine>();
    private bool isInitialized = false;

    // TODO: May be init from game manager?
    void Awake()
    {
        InitializeAudioGroups();
    }

    void Start()
    {
        if (playOnAwake)
        {
            StartAllGroups();
        }
    }

    void InitializeAudioGroups()
    {
        foreach (AudioGroup group in audioGroups)
        {
            // Create dedicated AudioSource for each group
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = group.loop;
            source.spatialBlend = group.spatialBlend;

            // Assign to mixer channel if mixer is set
            if (mixer != null)
            {
                AudioMixerGroup[] mixerGroups = mixer.FindMatchingGroups(group.mixerChannel);
                if (mixerGroups.Length > 0)
                {
                    source.outputAudioMixerGroup = mixerGroups[0];
                }
                else
                {
                    Debug.LogWarning($"Mixer channel '{group.mixerChannel}' not found in mixer for group '{group.groupName}'");
                }
            }

            group.audioSource = source;
        }

        isInitialized = true;
    }

    public void StartAllGroups()
    {
        if (!isInitialized)
        {
            Debug.LogError("AmbientConductor not initialized!");
            return;
        }

        foreach (AudioGroup group in audioGroups)
        {
            StartGroup(group);
        }
    }

    public void StartGroup(AudioGroup group)
    {
        if (group.audioClips.Count == 0)
        {
            Debug.LogWarning($"Audio group '{group.groupName}' has no clips assigned.");
            return;
        }

        if (!group.isPlaying)
        {
            Coroutine coroutine = StartCoroutine(AudioGroupCoroutine(group));
            activeCoroutines.Add(coroutine);
        }
    }

    public void StartGroup(string groupName)
    {
        AudioGroup group = audioGroups.Find(g => g.groupName == groupName);
        if (group != null)
        {
            StartGroup(group);
        }
        else
        {
            Debug.LogWarning($"Audio group '{groupName}' not found.");
        }
    }

    public void StopAllGroups()
    {
        foreach (AudioGroup group in audioGroups)
        {
            StopGroup(group);
        }

        // Stop all coroutines
        foreach (Coroutine coroutine in activeCoroutines)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        activeCoroutines.Clear();
    }

    public void StopGroup(AudioGroup group)
    {
        group.isPlaying = false;
        if (group.audioSource != null)
        {
            group.audioSource.Stop();
        }
    }

    public void StopGroup(string groupName)
    {
        AudioGroup group = audioGroups.Find(g => g.groupName == groupName);
        if (group != null)
        {
            StopGroup(group);
        }
        else
        {
            Debug.LogWarning($"Audio group '{groupName}' not found.");
        }
    }

    public void PauseAllGroups()
    {
        foreach (AudioGroup group in audioGroups)
        {
            if (group.audioSource != null && group.audioSource.isPlaying)
            {
                group.audioSource.Pause();
            }
        }
    }

    public void ResumeAllGroups()
    {
        foreach (AudioGroup group in audioGroups)
        {
            if (group.audioSource != null)
            {
                group.audioSource.UnPause();
            }
        }
    }

    private IEnumerator AudioGroupCoroutine(AudioGroup group)
    {
        group.isPlaying = true;

        // TODO: May be not needed 
        float initialDelay = group.GetRandomInterval();
        yield return new WaitForSeconds(initialDelay);

        while (group.isPlaying)
        {
            AudioClip clip = group.GetNextClip();

            if (clip != null)
            {
                // TODO: Make random volume as option
                group.audioSource.volume = group.GetRandomVolume();

                if (group.loop)
                {
                    group.audioSource.clip = clip;
                    group.audioSource.Play();
                }
                else
                {
                    group.audioSource.PlayOneShot(clip);
                }

                // Wait for sound to fully played before play next one
                if (group.waitUntilDone)
                {
                    yield return new WaitForSeconds(clip.length);
                }

                // Idk is it correct for loop case
                if (group.loop)
                {
                    yield break;
                }
            }

            float waitTime = group.GetRandomInterval();
            yield return new WaitForSeconds(waitTime);
        }
    }

    void OnDestroy()
    {
        StopAllGroups();
    }

    void OnDisable()
    {
        StopAllGroups();
    }
}