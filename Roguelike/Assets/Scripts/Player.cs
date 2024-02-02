using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator anim;
    private int food;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        food = GameManager.Instance.playerFoodPoints;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.Instance.playerFoodPoints = food;
    }

    private void Update()
    {
        if (!GameManager.Instance.playersTurn)
            return;

        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;

        base.AttemptMove<T>(xDir, yDir);

        CheckIfGameOver();
        GameManager.Instance.playersTurn = false;
    }

    void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        anim.SetTrigger("attack");
    }

    
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss)
    {
        anim.SetTrigger("hit");
        food -= loss;
        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Exit"))
        {
            Invoke(nameof(Restart), restartLevelDelay);
            enabled = false;
        }
        else if (collider.CompareTag("Food"))
        {
            food += pointsPerFood;
            collider.gameObject.SetActive(false);
        }
        else if (collider.CompareTag("Soda"))
        {
            food += pointsPerSoda;
            collider.gameObject.SetActive(false);
        }
    }
}
