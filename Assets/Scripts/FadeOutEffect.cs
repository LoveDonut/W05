using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutEffect : MonoBehaviour
{
    [SerializeField] Image fadeImage;    // Inspector에서 할당할 검은색 이미지
    public float fadeDuration = 2f; // 페이드 아웃이 진행될 시간

    private bool isFading = false;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0f;

        // 현재 이미지의 알파 값을 가져옵니다.
        Color fadeColor = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 0f;
        fadeImage.color = fadeColor;
        fadeImage.gameObject.SetActive(false); // 페이드 아웃이 끝나면 이미지 비활성화

        isFading = false;
    }
}
