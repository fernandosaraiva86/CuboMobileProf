using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }


    [Header ("Spawnar Objetos")]
    [SerializeField] private GameObject obstaclePrefab;
    public float spawnInterval = 2f;
    public bool isGameOver = false;
    public float spawnY = 11f;
    public float spawnX = 7f;

    [Header("Controle Mapeamento")]
    [SerializeField] private InputActionReference cancelAction;

    [Header("Menu Pause")]
    public GameObject pauseMenu;

    [Header("Pontuação")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
    private float timeScore = 0f;

    private void OnEnable()
    {
        cancelAction.action.Enable();

        // Registrar evento
        cancelAction.action.performed += OnCancel;
    }

    private void OnDisable()
    {
        // Remover evento
        cancelAction.action.performed -= OnCancel;

        cancelAction.action.Disable();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (isGameOver) { return; }//Não pode pausar o jogo se o game over for true
        
        if(Time.timeScale == 0f)
        {
            StartCoroutine(ScaleTime(0f, 1f, 0.5f));
            pauseMenu.SetActive(false);
        }
        else if(Time.timeScale == 1f)
        {
            StartCoroutine(ScaleTime(1f, 0f, 0.5f));
            pauseMenu.SetActive(true);
        }
    }

    void Awake()
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
        StartCoroutine(SpawnObstacle());
    }

    void Update()
    {
        if (isGameOver) { return; }//Não pode pausar o jogo se o game over for true
        Pontuacao();
    }

    private IEnumerator SpawnObstacle()
    {
        while (!isGameOver) { 

         var obstacleSpawn = Random.Range(1, 4);

            for (int i = 0; i < obstacleSpawn; i++)//Cria laço para spawnar mais de um obstáculo por vez
            {

                var xPosition = Random.Range(-spawnX, spawnX);

                var damping = Random.Range(0f, 2f);//Resitência do AR no objeto

                var objObstacle =
                Instantiate(obstaclePrefab,
                new Vector3(xPosition, spawnY, 0), Quaternion.identity);

                objObstacle.GetComponent<Rigidbody>().linearDamping = damping;
            }

                yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator ScaleTime(float start, float end, float duration)
    {
       float lastTime = Time.realtimeSinceStartup;
       float timer = 0.0f;

        while (timer < duration) {
        //Interpolação e suavisação do tempo
        Time.timeScale = Mathf.Lerp(start, end, timer / duration);

        //Ajustar e manter a concistência do tempo de física
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        timer += Time.realtimeSinceStartup - lastTime;
        lastTime = Time.realtimeSinceStartup;

         yield return null;
        }

        Time.timeScale = end;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

    }

    private void Pontuacao()
    {
        timeScore += Time.deltaTime;
        if (timeScore >= 1f) {
            score++;
            scoreText.text = "Score: " + score;
            timeScore = 0f;
        }
    }

    //Pode ser acessado de outros scripts
    public void Enable()
    {
        gameObject.SetActive(true);
    }
}

