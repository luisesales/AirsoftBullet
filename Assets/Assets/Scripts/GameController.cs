using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject hudCanvas;
    private TheWallController theWall;
    private TMPro.TextMeshProUGUI timer;
    private TMPro.TextMeshProUGUI ammo;
    private TMPro.TextMeshProUGUI points;
    private float time;

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

    }

    private void StartHudCanvas()
    {
        hudCanvas = GameObject.FindWithTag("HUDCanvas");
        timer = hudCanvas.transform.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
        timer.gameObject.SetActive(false);
        ammo = hudCanvas.transform.Find("Ammo").GetComponent<TMPro.TextMeshProUGUI>();
        points = hudCanvas.transform.Find("Points").GetComponent<TMPro.TextMeshProUGUI>();
        time = 0f;
    }
    private void StartGame()
    {
        StartHudCanvas();
        pointsCount = 0;
        //UpdatePoints(pointsCount);
        theWall = GameObject.FindWithTag("TheWall").GetComponent<TheWallController>();        
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

}
