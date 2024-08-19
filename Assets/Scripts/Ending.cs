using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Ending : MonoBehaviour
{
    public Image fadeImage; // ��� Image ������Ʈ
    public float fadeDuration = 2f; // ���̵��� ���� �ð�
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

        // �̹����� �ؽ�Ʈ�� ���̵���
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(1, 1, 1, alpha); // �̹��� ���̵���

            yield return null;
        }

        isEnd = true;
        // Ȯ���ϰ� ���ĸ� 1�� ����
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
