using UnityEngine;
using UnityEngine.SceneManagement;  // Necesario para manejar escenas

public class FinishMarker : MonoBehaviour
{
    /*private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
            //Debug.Log("¡Nivel completado! Cargando siguiente escena...");
            SceneManager.LoadScene("SceneWin");
        }
    }*/
    [Tooltip("Nombre exacto de la escena a la que quieres cambiar.")]
    [SerializeField] string targetSceneName;

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos que sea el jugador (u otro objeto con tag “Player”)
        if (other.CompareTag("Player"))
        {
            // Llamamos al método ChangeLevel del GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadScene(targetSceneName);
            }
            else
            {
                Debug.LogError("LevelTrigger: No se encontró una instancia de GameManager en la escena.");
            }
        }
    }
}
