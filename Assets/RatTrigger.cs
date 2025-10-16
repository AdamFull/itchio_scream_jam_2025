using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatTrigger : MonoBehaviour
{
    [Header("Child Object")]
    [Tooltip("The child object that will move between points")]
    public Transform childObject;

    [Header("Movement Settings")]
    [Tooltip("Speed of the child object movement")]
    public float moveSpeed = 5f;

    [Tooltip("Starting point of movement")]
    public Transform startPoint;

    [Tooltip("End point of movement")]
    public Transform endPoint;

    [Header("Probability")]
    [Tooltip("Chance (0-1) that the effect will trigger when player enters")]
    [Range(0f, 1f)]
    public float activationProbability = 0.5f;

    [Header("Audio")]
    [Tooltip("Optional: Audio source to play when triggered")]
    public AudioSource audioSource;

    private bool hasBeenTriggered = false;
    private bool isMoving = false;
    private float travelTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (childObject != null)
        {
            childObject.gameObject.SetActive(false);
            childObject.localPosition = startPoint.position;
        }
        else
        {
            Debug.LogWarning("Child object not assigned in " + gameObject.name);
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered && childObject != null)
        {
            // Check probability
            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= activationProbability)
            {
                ActivateEffect();
            }
            else
            {
                Debug.Log("Activation failed - probability check");
            }

            hasBeenTriggered = true;
        }
    }

    private void ActivateEffect()
    {
        childObject.gameObject.SetActive(true);
        childObject.localPosition = startPoint.position;

        float distance = Vector3.Distance(startPoint.position, endPoint.position);
        travelTime = distance / moveSpeed;

        isMoving = true;

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        Destroy(gameObject, travelTime + 0.5f);
    }

     // Update is called once per frame
     void Update()
    {
        if (isMoving && childObject != null)
        {
            childObject.localPosition = Vector3.MoveTowards(
                childObject.localPosition,
                endPoint.position,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(childObject.localPosition, endPoint.position) < 0.01f)
            {
                isMoving = false;
            }
        }
    }
}
