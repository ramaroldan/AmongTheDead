using UnityEngine;
using UnityEngine.SceneManagement;

public class BackLevel : MonoBehaviour
{
    public void ReturnGame()
    {
        Debug.Log("BackLevel");
        SceneManager.LoadScene("TP_2");
    }
}
