using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Ending : MonoBehaviour
{
    public Image fadeImage; // 흰색 Image 컴포넌트
    public float fadeDuration = 2f; // 페이드인 지속 시간
    bool isEnd = false;

    private void Start()
    {
        fadeImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if(isEnd)
        {
            QuitGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        // 이미지와 텍스트를 페이드인
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(1, 1, 1, alpha); // 이미지 페이드인

            yield return null;
        }

        isEnd = true;
        // 확실하게 알파를 1로 설정
        fadeImage.color = new Color(1, 1, 1, 1);
    }

    public void QuitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }
    }
}
