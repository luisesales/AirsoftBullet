using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject hudCanvas;
    private TheWallController theWall;
    private TMPro.TextMeshProUGUI timer;
    private TMPro.TextMeshProUGUI ammo;
    private TMPro.TextMeshProUGUI points;
    private TMPro.TextMeshProUGUI countdownTime;
    private float time, remainderTime;
    [SerializeField] 
    private float countdownTimer;

    private int pointsCount;

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
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 0f)
        {
            time -= Time.deltaTime;
            timer.text = Mathf.CeilToInt(time).ToString();
        }
        else if (timer.gameObject.activeSelf)
        {
            timer.gameObject.SetActive(false);
        }

        if(remainderTime >= 0)
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

    private void StartHudCanvas()
    {
        hudCanvas = GameObject.FindWithTag("HUDCanvas");
        timer = hudCanvas.transform.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
        timer.gameObject.SetActive(false);
        ammo = hudCanvas.transform.Find("Ammo").GetComponent<TMPro.TextMeshProUGUI>();
        points = hudCanvas.transform.Find("Points").GetComponent<TMPro.TextMeshProUGUI>();
        countdownTime = hudCanvas.transform.Find("CountdownTimer").GetComponent<TMPro.TextMeshProUGUI>();
        time = 0f;
    }
    private void StartGame()
    {
        StartHudCanvas();
        pointsCount = 0;
        //UpdatePoints(pointsCount);
        theWall = GameObject.FindWithTag("TheWall").GetComponent<TheWallController>();
        StartCoroutine(CountdownTimer(countdownTimer));        
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

    public void UpdateCountdownTimer(int currentAmmo, int magazineSize, int magazineAmount)
    {
        if (hudCanvas == null)
        {
            StartHudCanvas();
        }
        ammo.text = currentAmmo + " / " + magazineSize + " (" + magazineAmount + ")";
    }

    public void UpdateAmmoCount(int currentAmmo, int magazineSize, int magazineAmount)
    {
        if (hudCanvas == null)
        {
            StartHudCanvas();
        }
        ammo.text = currentAmmo + " / " + magazineSize + " (" + magazineAmount + ")";
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

        theWall.SetCanContinue(false);
    }
}
