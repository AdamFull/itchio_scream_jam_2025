using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutEffect : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 3f;
    private Image _fadeImage;
    private float _timer = 0f;
    private bool _isFading = false;
    void OnEnable()
    {
        _fadeImage = GetComponent<Image>();
        if (_fadeImage != null)
        {
            Color color = _fadeImage.color;
            color.a = 1;
            _fadeImage.color = color;
            _isFading = true;
        }
    }
    
    void Update()
    {
        if (_fadeImage != null && _isFading)
        {
            _timer += Time.deltaTime;
            
            float delta = Mathf.Lerp(1f, 0f, _timer / _fadeDuration);
            Color color = _fadeImage.color;
            color.a = delta;
            _fadeImage.color = color;

            if (_timer >= _fadeDuration)
            {
                _isFading = false;
            }
        }
    }

    void OnDisable()
    {
        _timer = 0f;
    }
}

