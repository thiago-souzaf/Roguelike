using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }
    #endregion

    public float startLevelDelay = 2f;
    public int playerFoodPoints = 100;
    public float turnDelay = 0.1f;
    [HideInInspector] public bool playersTurn = false;

    private Text levelTxt;
    private GameObject levelPanel;
    private BoardManager boardScript;
	private int level;
    private List<Enemy> enemiesList;
    private bool enemiesMoving = false;
    private bool doingSetup;
    private bool isFirstLevel = true;

    void Start()
    {
        
    }

    private void OnLevelWasLoaded(int level)
    {
        if (isFirstLevel)
        {
            isFirstLevel = false;
            this.level = 1;
            boardScript = GetComponent<BoardManager>();
            enemiesList = new List<Enemy>();
            InitGame();
            return;
        }
        this.level++;
        InitGame();
    }
    void InitGame()
    {
        playersTurn = false;
        doingSetup = true;

        levelPanel = GameObject.Find("LevelPanel");
        levelTxt = levelPanel.GetComponentInChildren<Text>();
        levelTxt.text = "Day " + level;
        levelPanel.SetActive(true);

        Invoke(nameof(HideLevelPanel), startLevelDelay);
        boardScript.SetupScene(level);
        enemiesList.Clear();
    }

    void HideLevelPanel()
    {
        levelPanel.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelTxt.text = "After " + level + " days, you died.";
        levelPanel.SetActive(true);
        enabled = false;
    }

    private void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
            return;
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemiesList.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemiesList.Count == 0)
            yield return new WaitForSeconds(turnDelay);
        
        for (int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].MoveEnemy();
            yield return new WaitForSeconds(turnDelay);
        }
        playersTurn = true;
        enemiesMoving = false;

    }
    
}
