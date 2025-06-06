using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuSystem : MonoBehaviour
{
    

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
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
