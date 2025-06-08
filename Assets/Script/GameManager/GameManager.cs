using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject transitionImage1;
    [SerializeField] GameObject transitionImage2;
    [SerializeField] float transitionTime1 = 3f;
    [SerializeField] float transitionTime2 = 3f;
    //[SerializeField] TextMeshProUGUI textScore;

    //[Header("Enemys:")]
    //[SerializeField] Transform[] positions; //array de posiciones (empty gameObjects enemies)
    //[SerializeField] GameObject[] enemyPrefab; //prefab de enemigo
    //[SerializeField] Transform parentEnemies; //object vacio para guardar clones de enemigos

    //[Range(2, 6)]
    //[SerializeField] float time; //tiempo de aparicion de enemigos

    //int score; //puntaje total

    //[Space]

    public static GameManager Instance { get; private set; } //singleton

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //InvokeRepeating("CreateEnemy", time, time); //se invoca cada cierto tiempo
        
    }

   /* void CreateEnemy()
    {
        int pos = Random.Range(0, positions.Length); //selecciona una posicion aleatoria
        int enemy = Random.Range(0, enemyPrefab.Length); //selecciona un enemigo aleatorio
        GameObject cloneEnemy= Instantiate(enemyPrefab[enemy], positions[pos].position, positions[pos].rotation); //clonamos el enemigo en la posicion aleatoria
        cloneEnemy.transform.SetParent(parentEnemies); //guardamos clones en el padre
    }*/

    public void GameOver()
    {
        panelGameOver.SetActive(true);
    }

   /* public void LoadScene(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("GameManager: ChangeLevel recibió un nombre de escena vacío.");
            return;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }*/

    public void LoadScene(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("GameManager: ChangeLevel recibió un nombre de escena vacío.");
            return;
        }
        StartCoroutine(LoadSceneWithTransition(name));
    }

    private IEnumerator LoadSceneWithTransition(string sceneName)
    {
        if (transitionImage1 != null)
            transitionImage1.SetActive(true);

        if (transitionImage2 != null)
            transitionImage2.SetActive(false);

        yield return new WaitForSeconds(transitionTime1);

        if (transitionImage1 != null)
            transitionImage1.SetActive(false);

        if (transitionImage2 != null)
            transitionImage2.SetActive(true);

        yield return new WaitForSeconds(transitionTime2);

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    //funcion que vamos a llamar desde el script de salud del enemigo cuando muera 
    /*public void ScoreEnemy(int scoreValue)
    {
        score += scoreValue;
        textScore.text = "Score: " + score.ToString();
    }*/
}
