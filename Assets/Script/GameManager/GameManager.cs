using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private MainCharacterMove _playerMove;

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
        Invoke("ResetPlayerPosition", 5f);

    }

    private void ResetPlayerPosition()
    {
        _playerMove = FindObjectOfType<MainCharacterMove>();
        _playerMove.ResetPositionAndRotation();
    }

    public void ObtainGOPanel(GameObject panel)
    {
        panelGameOver = panel;
    }

    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];

            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }

        return false;
    }

    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        return raysastResults;
    }

    public bool CheckMouseOverUI()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

}