using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuSystem : MonoBehaviour
{

    public void StartGame()
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

    public void Option()
    {
        SceneManager.LoadScene("Options");
    }

    public void Instruction()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
