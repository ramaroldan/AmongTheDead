using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private MainCharacterMove _playerMove;
    [SerializeField] private GameObject inventary;

    //blic TextMeshProUGUI findObjectiveText;
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


        // Buscar el panel de Game Over por tag si no está asignado
        if (panelGameOver == null)
        {
            panelGameOver = GameObject.FindWithTag("panelGameOver");
            if (panelGameOver == null)
            {
                Debug.LogError("Panel Game Over no asignado ni encontrado por tag en GameManager.");
                return;
            }
        }
        inventary = GameObject.FindWithTag("Inventary");
        inventary.SetActive(false);
        panelGameOver.SetActive(true);
    }


    /// Calcula el índice de la siguiente escena en BuildSettings y la arranca con transición.
    public void LoadNextScene()
    {
        //findObjectiveText.text = "Find Your Wife Elena";
        // Obtener el índice de la escena actual
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Cargar la siguiente escena por índice
        SceneManager.LoadScene(currentSceneIndex + 1);
        Invoke("ResetPlayerPosition", 5f);
        
    }

    private void ResetPlayerPosition()
    {
        _playerMove = FindObjectOfType<MainCharacterMove>();
        _playerMove.ResetPositionAndRotation();
    }

    private void LateUpdate()
    {
        
    }
}
