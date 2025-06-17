using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}
