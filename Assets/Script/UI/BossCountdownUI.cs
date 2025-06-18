using System.Collections;
using UnityEngine;
using TMPro;              // si usas TextMeshPro, comenta la línea anterior y descomenta esta

public class BossCountdownUI : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI countdownText;      // o TMP_Text si usas TextMeshPro
                                                      // [SerializeField] private TMP_Text countdownText;

    [Header("Boss Generator")]
    [SerializeField] private ProceduralForestGenerator forestGenerator;

    private float timer;

    private void Start()
    {
        if (forestGenerator == null)
        {
            Debug.LogError("Asigna ProceduralForestGenerator en el inspector.");
            enabled = false;
            return;
        }
        // toma el tiempo de respawn inicial
        timer = forestGenerator.bossRespawnTime;
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
            // Muestra el texto y resetea el temporizador
            countdownText.gameObject.SetActive(true);
            timer = forestGenerator.bossRespawnTime;

            // Cuenta regresiva
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                // Actualiza el texto (usamos Ceil para mostrar segundos enteros)
                countdownText.text = $"¡TERROR IS APPROACHING IN {Mathf.Ceil(timer)} s!";
                yield return null;
            }

            // Aviso de llegada
            countdownText.text = "¡BE CAREFUL!";

            // Espera un segundo para que el jugador lea el aviso
            yield return new WaitForSeconds(1f);

            // Oculta el texto hasta la próxima cuenta
            countdownText.gameObject.SetActive(false);

            // Ahora esperamos internamente al spawn real del boss
            // (el ProceduralForestGenerator hará el spawn con su coroutine)
            // Aquí simplemente volvemos a iniciar otro ciclo
    }
}
