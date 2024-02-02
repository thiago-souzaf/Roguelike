using UnityEngine;

public class Wall : MonoBehaviour
{
	public Sprite dmgSprite;
	public int hp = 4;
    public AudioClip chopSound1;
    public AudioClip chopSound2;

	private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        spriteRenderer.sprite = dmgSprite;
        SoundManager.Instance.RandomizeSfx(chopSound1, chopSound2);
        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
