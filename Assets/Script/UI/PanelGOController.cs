using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGOController : MonoBehaviour
{
    [SerializeField] GameObject panelGameOver;
    private void Awake()
    { 
        DontDestroyOnLoad(gameObject);
    }
}
