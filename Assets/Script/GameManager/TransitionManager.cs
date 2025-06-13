using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    [SerializeField] CanvasGroup image1;
    [SerializeField] CanvasGroup image2;
    [SerializeField] float timeImage1 = 2f;
    [SerializeField] float timeImage2 = 2f;
    [SerializeField] float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(PlayTransition());
    }

    IEnumerator PlayTransition()
    {
        // Imagen 1 con fade
        yield return StartCoroutine(FadeImage(image1, true));
        yield return new WaitForSeconds(timeImage1);
        yield return StartCoroutine(FadeImage(image1, false));

        // Imagen 2 con fade
        yield return StartCoroutine(FadeImage(image2, true));
        yield return new WaitForSeconds(timeImage2);
        yield return StartCoroutine(FadeImage(image2, false));

        // Cargar la siguiente escena en la Build
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }

    IEnumerator FadeImage(CanvasGroup canvasGroup, bool fadeIn)
    {
        float startAlpha = fadeIn ? 0 : 1;
        float endAlpha = fadeIn ? 1 : 0;
        float timer = 0f;

        canvasGroup.gameObject.SetActive(true);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (!fadeIn)
            canvasGroup.gameObject.SetActive(false);
    }
}


