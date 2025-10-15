using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    private GameObject hudCanvas;
    private GameObject gameOverCanvas;
    private Button restart;
    private TheWallController theWall;
    private TMPro.TextMeshProUGUI timer;
    private TMPro.TextMeshProUGUI ammo;
    private TMPro.TextMeshProUGUI points;
    private TMPro.TextMeshProUGUI countdownTime;
    private TMPro.TextMeshProUGUI currentBackspin;
    private float time, remainderTime;
    [SerializeField] 
    private float countdownTimer;

    private int pointsCount;

    private PlayerController playerController;
    
    private bool gameStarted = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        RestartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            PrintCurrentBackspin();
            if (time >= 0f)
            {
                time -= Time.deltaTime;
                timer.text = Mathf.CeilToInt(time).ToString();
            }
            else if (timer.gameObject.activeSelf)
            {
                timer.gameObject.SetActive(false);
            }

            if (remainderTime >= 0)
            {
                int minuts = (int)remainderTime / 60;
                int seconds = (int)remainderTime % 60;
                if (seconds < 10)
                {
                    countdownTime.text = $"{minuts}:0{seconds}";
                }
                else
                {
                    countdownTime.text = $"{minuts}:{seconds}";
                }

            }
        }
    }

    private void StartHudCanvas()
    {
        hudCanvas = GameObject.FindWithTag("HUDCanvas");
        timer = hudCanvas.transform.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
        timer.gameObject.SetActive(false);
        ammo = hudCanvas.transform.Find("Ammo").GetComponent<TMPro.TextMeshProUGUI>();
        points = hudCanvas.transform.Find("Points").GetComponent<TMPro.TextMeshProUGUI>();
        countdownTime = hudCanvas.transform.Find("CountdownTimer").GetComponent<TMPro.TextMeshProUGUI>();
        time = 0f;
        currentBackspin = hudCanvas.transform.Find("CurrentBackspin").GetComponent<TMPro.TextMeshProUGUI>();
    }
    private void StartGame(Scene scene, LoadSceneMode mode)
    {
        StartHudCanvas();

        pointsCount = 0;
        theWall = GameObject.FindWithTag("TheWall").GetComponent<TheWallController>();
        StartCoroutine(CountdownTimer(countdownTimer));
        gameOverCanvas = GameObject.FindWithTag("GameOverCanvas");
        restart = gameOverCanvas.transform.Find("Restart").GetComponent<Button>();
        if(restart != null)
        {
            restart.onClick.AddListener(RestartGame);
        }
        gameOverCanvas.SetActive(false);
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        gameStarted = true;
        SceneManager.sceneLoaded -= StartGame;
        remainderTime = countdownTimer;
    }

    public void StartTimer(float time)
    {
        if (hudCanvas == null)
        {
            StartHudCanvas();
        }
        timer.gameObject.SetActive(true);
        this.time = time;
    }

    public void StartCountdownTimer(float time)
    {
        if (hudCanvas == null)
        {
            StartHudCanvas();
        }
        countdownTime.gameObject.SetActive(true);
        this.time = time;
    }

    public void PrintCurrentBackspin()
    {
        currentBackspin.text = $"{playerController.GetCurrentBackspin()}";
    }

    public void UpdateAmmoCount(int currentAmmo, int magazineSize, int magazineAmount, bool isAutomatic)
    {
        if (hudCanvas == null)
        {
            StartHudCanvas();
        }
        string auto = isAutomatic ? "III(A) " : "I(S) ";
        ammo.text = auto+  currentAmmo + " / " + magazineSize + " (" + magazineAmount + ")";
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= StartGame;
    }
    
    private void EndGame()
    {
        gameOverCanvas.SetActive(true);
        gameOverCanvas.transform.Find("Points").GetComponent<TMPro.TextMeshProUGUI>().text = pointsCount.ToString();
        theWall.SetCanContinue(false);
        Camera.main.gameObject.GetComponent<CameraController>().UnlockCursor();
        gameStarted = false;
    }


    public void UpdatePoints(int value, GameObject target)
    {
        if (hudCanvas == null)
        {
            StartHudCanvas();
        }
        pointsCount += value;
        points.text = pointsCount.ToString();
        theWall.DestroyTarget(target);
    }

    IEnumerator CountdownTimer(float countdownTimer)
    {
        remainderTime = countdownTimer;
        int increase = 3, maxTargets = 5, timeToCompare = 3;

        while (remainderTime > 0)
        {
            int minuts = (int)(remainderTime / 60);

            if (timeToCompare == minuts)
            {
                theWall.SetMaxTarget(maxTargets + increase);
                maxTargets += increase;
                increase--;
                timeToCompare--;
            }

            yield return new WaitForSeconds(1f);
            remainderTime--;
        }
        EndGame();
    }

    public void RestartGame()
    {
        Debug.Log("Restarting Game");
        SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Debug.Log("Loading Game");
        SceneManager.sceneLoaded += StartGame;
        Debug.Log("StartGame");
    }
}
