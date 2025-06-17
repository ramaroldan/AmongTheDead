using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextFind : MonoBehaviour
{
    public TextMeshProUGUI findObjectiveText;

    private void LateUpdate()
    {
        if(SceneManager.GetActiveScene().name == "Level2")
        {
            findObjectiveText.text = "Find Your Wife Elena";
        }
    }
}
