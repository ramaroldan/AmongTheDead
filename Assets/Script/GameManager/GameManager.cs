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
        // Pausar el juego
        //Time.timeScale = 0f;

        // Destruir todos los enemigos 
        foreach (var enemy in FindObjectsOfType<EnemyHealth>())
            Destroy(enemy.gameObject);

        // Mostrar el panel de Game Over
        if (panelGameOver == null)
        {
            Debug.LogError("Panel Game Over no asignado en GameManager.");
            return;
        }
        panelGameOver.SetActive(true);
    }


    /// Calcula el índice de la siguiente escena en BuildSettings y la arranca con transición.
    public void LoadNextScene()
    {
        // Obtener el índice de la escena actual
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Cargar la siguiente escena por índice
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
