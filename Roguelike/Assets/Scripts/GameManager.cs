using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int playerFoodPoints = 100;
    public float turnDelay = 0.1f;
    [HideInInspector] public bool playersTurn = true;

    private BoardManager boardScript;
	private int level = 3;
    private List<Enemy> enemiesList;
    private bool enemiesMoving = false;

    void Start()
    {
        boardScript = GetComponent<BoardManager>();
        enemiesList = new List<Enemy>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
        enemiesList.Clear();
    }

    public void GameOver()
    {
        enabled = false;
    }

    private void Update()
    {
        if (playersTurn || enemiesMoving)
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
