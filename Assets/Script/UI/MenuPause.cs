using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{

    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuQuit;
    bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            if (isPaused == false)
            {
                menuPause.SetActive(true);
                isPaused = true;

                Time.timeScale = 0;
                Cursor.visible = true; //Muestra el cursor
                                       // Cursor.lockState = CursorLockMode.None; //Desactiva el cursor

                AudioSource[] sounds = FindObjectsOfType<AudioSource>(); //Busca todos los sonidos
                for (int i = 0; i < sounds.Length; i++)
                {
                    sounds[i].Pause(); //Pausa todos los sonidos
                }
            }
            else if (isPaused == true)
            {
                ResumeGame();
            }
        }
    }

    public void ResumeGame()
    {
        menuPause.SetActive(false);
        menuQuit.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
        Cursor.visible = false; //Oculta el cursor
        // Cursor.lockState = CursorLockMode.Locked; //Activa el cursor
        
        /*AudioSource[] sounds = FindObjectsOfType<AudioSource>(); //Busca todos los sonidos
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Play(); //reanuda todos los sonidos
        }*/
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
