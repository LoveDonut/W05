using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutEffect : MonoBehaviour
{
    [SerializeField] Image fadeImage;    // Inspector���� �Ҵ��� ������ �̹���
    public float fadeDuration = 2f; // ���̵� �ƿ��� ����� �ð�

    private bool isFading = false;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0f;

        // ���� �̹����� ���� ���� �����ɴϴ�.
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
        fadeImage.gameObject.SetActive(false); // ���̵� �ƿ��� ������ �̹��� ��Ȱ��ȭ

        isFading = false;
    }
}
