using System.Collections;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
	public float moveTime = .1f;
	public LayerMask blockingLayer;
	public bool canMove;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb;
	private float inverseMoveTime;

	protected bool hasFinishedMove;

	protected virtual void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f / moveTime;
		hasFinishedMove = true;
	}

	protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 startPosition = transform.position;
		Vector2 end = startPosition + new Vector2(xDir, yDir);

		boxCollider.enabled = false;
		hit = Physics2D.Linecast(startPosition, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null)
		{
            StartCoroutine(SmoothMovement(end));
			return true;
		}
		return false;
	}

	protected IEnumerator SmoothMovement(Vector3 endPosition)
	{

		hasFinishedMove = false;
		float sqrRemainingDistance = (transform.position - endPosition).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon)
		{
			Vector3 newPosition = Vector3.MoveTowards(rb.position, endPosition, inverseMoveTime * Time.deltaTime);
			rb.MovePosition(newPosition);
			sqrRemainingDistance = (transform.position - endPosition).sqrMagnitude;
			yield return null;
		}
		hasFinishedMove = true;
	}

	protected virtual void AttemptMove<T>(int xDir, int yDir)
		where T: Component
	{
        canMove = Move(xDir, yDir, out RaycastHit2D hit);

        if (hit.transform == null)
		{
			return;
		}

		T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove  && hitComponent != null)
        {
			OnCantMove(hitComponent);
        }
    }

	protected abstract void OnCantMove <T> (T component)
		where T : Component;
}
