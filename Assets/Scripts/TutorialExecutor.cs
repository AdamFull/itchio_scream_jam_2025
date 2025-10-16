using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExecutor : MonoBehaviour
{
    public float delayInSeconds = 3f;
    public float fadeOutDuration = 0.5f;
    public float fadeInDuration = 0.5f;
    public bool hideOnStart = true;

    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;
    private bool isAnyKeyPressed = false;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (hideOnStart)
        {
            StartCoroutine(HideAfterDelay());
        }
    }

    private void Update()
    {
        if(!isAnyKeyPressed)
        {
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.F) ||
                Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                isAnyKeyPressed = true;
                StartCoroutine(HideAfterDelay());
            }
        }
    }

    public void ActivateHideTimer()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        if (canvasGroup.alpha < 1f)
        {
            yield return StartCoroutine(FadeIn());
        }

        yield return new WaitForSeconds(delayInSeconds);

        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeOutDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator FadeIn()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeInDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public void ShowUI()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    public void HideUI()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOut());
    }
}
