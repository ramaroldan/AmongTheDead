using UnityEngine;
using TMPro;

public class DistanceDisplay : MonoBehaviour
{
    private Transform player;
    public TMP_Text distanceText;

    public float unitsToMeters = 1f; // unidades a metros
    public int decimals = 0; // decimales a mostrar

    private Transform target;
    

    void Update()
    {
        if(player == null)
        {
            GameObject plyr = GameObject.FindGameObjectWithTag("Player");
            if(plyr != null )
            {
                player = plyr.transform;
            }
            else
            {
                return;
            }
        }
        if (target == null)
        {
            GameObject targetObj = GameObject.FindGameObjectWithTag("Finish");
            if (targetObj != null)
            {
                target = targetObj.transform;
            }
            else
            {
                return; // todavía no está, salimos
            }
        }

        if (player == null || distanceText == null) return;
        
        float distance = Vector3.Distance(player.position, target.position); // distancia en unidades
        float distanceInMeters = distance * unitsToMeters; // distancia en metros

        string meters = distanceInMeters.ToString($"F{decimals}");
        distanceText.text = $"Distance to target: {meters} m";
    }


    
}
