using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject panelGameOver;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        panelGameOver.SetActive(true);
    }

    /// <summary>
    /// Calcula el índice de la siguiente escena en BuildSettings y la arranca con transición.
    /// </summary>
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextIndex = currentIndex + 1;

        if (nextIndex >= totalScenes)
        {
            Debug.LogWarning($"GameManager: No hay más escenas en BuildSettings (índice {nextIndex}).");
            return;
        }

        TransitionManager.Instance.PlayTransition(nextIndex);
    }
}
