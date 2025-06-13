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
    /// Calcula el �ndice de la siguiente escena en BuildSettings y la arranca con transici�n.
    /// </summary>
    public void LoadNextScene()
    {
        // Obtener el �ndice de la escena actual
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Cargar la siguiente escena por �ndice
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
