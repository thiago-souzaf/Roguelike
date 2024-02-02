using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    [Header("Audio clips")]
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip deathSound;

    [Space]
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;

    private Animator anim;
    private int food;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        food = GameManager.Instance.playerFoodPoints;
        foodText.text = "Food: " + food;

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
        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);
        if (canMove)
        {
            SoundManager.Instance.RandomizeSfx(moveSound1, moveSound2);
        }
        GameManager.Instance.playersTurn = false;

        CheckIfGameOver();
    }

    void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager.Instance.PlaySfx(deathSound);
            SoundManager.Instance.musicSource.Stop();
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
        foodText.text = " - " + loss + " Food: " + food;

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
            foodText.text = " + " + pointsPerFood + " Food: " + food;
            SoundManager.Instance.RandomizeSfx(eatSound1, eatSound2);

            collider.gameObject.SetActive(false);
        }
        else if (collider.CompareTag("Soda"))
        {
            food += pointsPerSoda;
            foodText.text = " + " + pointsPerSoda + " Food: " + food;
            SoundManager.Instance.RandomizeSfx(drinkSound1, drinkSound2);


            collider.gameObject.SetActive(false);
        }
    }
}
