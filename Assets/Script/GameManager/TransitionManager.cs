using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [Header("Transiciones")]
    [SerializeField] private GameObject[] transitionImages;
    [SerializeField] private float[] transitionTimes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// Inicia la transición y al final carga la escena cuyo índice pase por parámetro.
    /// </summary>
    public void PlayTransition(int buildIndex)
    {
        StartCoroutine(DoTransition(buildIndex));
    }

    private IEnumerator DoTransition(int buildIndex)
    {
        // Primer frame
        if (transitionImages.Length > 0 && transitionImages[0] != null)
            transitionImages[0].SetActive(true);
        if (transitionTimes.Length > 0)
            yield return new WaitForSeconds(transitionTimes[0]);

        // Segundo frame
        if (transitionImages.Length > 0 && transitionImages[0] != null)
            transitionImages[0].SetActive(false);
        if (transitionImages.Length > 1 && transitionImages[1] != null)
            transitionImages[1].SetActive(true);
        if (transitionTimes.Length > 1)
            yield return new WaitForSeconds(transitionTimes[1]);

        // Carga de la escena por índice
        SceneManager.LoadScene(buildIndex);
    }
}
