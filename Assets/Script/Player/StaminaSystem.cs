using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrain = 25f;
    public float staminaRegen = 20f;
    public float regenDelay = 3f;

    public Slider staminaSlider;

    private bool isTired = false;
    private float regenTimer = 0f;
    private bool isRunning = false;

    public bool CanRun => !isTired && currentStamina > 0;

    private void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    private void Update()
    {
        staminaSlider.value = currentStamina;

        if (!isRunning)
        {
            if (isTired)
            {
                regenTimer += Time.deltaTime;
                if (regenTimer >= regenDelay)
                {
                    RegenerateStamina();
                }
            }
            else
            {
                RegenerateStamina();
            }
        }
    }

    public void UseStamina()
    {
        isRunning = true;
        regenTimer = 0f;

        currentStamina -= staminaDrain * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        if (currentStamina <= 0)
        {
            isTired = true;
            regenTimer = 0f;
        }
    }

    public void StopRunning()
    {
        isRunning = false;
    }

    private void RegenerateStamina()
    {
        currentStamina += staminaRegen * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        if (currentStamina >= maxStamina)
        {
            isTired = false;
            regenTimer = 0f;
        }
    }
}
