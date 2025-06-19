using System.Collections;
using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject loadingPanel;      // El panel completo
    [SerializeField] private TextMeshProUGUI loadingText;  // El texto “Loading…”

    [Header("Configuración")]
    [SerializeField] private float displayTime = 3f;       // Segundos a mostrar

    private void Awake()
    {
        StartCoroutine(ShowLoading());
    }

    private IEnumerator ShowLoading()
    {
        loadingPanel.SetActive(true);

        // Animar el “…”  
        float t = 0f;
        while (t < displayTime)
        {
            int dots = ((int)(t * 2)) % 4;  // 0 a 3 puntos
            loadingText.text = "Loading" + new string('.', dots);
            t += Time.deltaTime;
            yield return null;
        }

        loadingPanel.SetActive(false);
    }
}
