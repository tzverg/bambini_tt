using UnityEngine;

public class SpiderC : MotionController
{
    [SerializeField] private float maxOffsetFromPlayer;
    [SerializeField] private float offsetFreeMotion;

    [SerializeField] private Transform playerTR;

    private Vector3 nextPosition;
    private float randomDistance;
    private float curOffsetFromPlayer;
    private bool finishMotion = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("StonePart"))
        {
            if (Random.Range(0F, 1F) > 0.5F)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    protected override void Move()
    {
        if (playerTR != null && finishMotion)
        {
            curOffsetFromPlayer = GetCurrentOffset();

            if (Vector3.Distance(transform.position, playerTR.position) > curOffsetFromPlayer)
            {
                MoveToPlayer();
            }
            else
            {
                MoveOnRandomDirection();
            }
        }
        else
        {
            MoveOnRandomDirection();
        }
    }

    private float GetCurrentOffset()
    {
        var randomSeed = Random.Range(0F, 1F);

        if (randomSeed > 0.3F)
        {
            return 0.01F;
        }
        else
        {
            return Random.Range(3F, maxOffsetFromPlayer);
        }
    }

    private void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTR.position, maxSpeed * Time.deltaTime);
    }

    private void MoveOnRandomDirection()
    {
        if (finishMotion)
        {
            nextPosition = transform.position;

            nextPosition.x = Random.Range(-offsetFreeMotion, offsetFreeMotion);
            nextPosition.y = Random.Range(-offsetFreeMotion, offsetFreeMotion);

            BoundaryClamp(ref nextPosition);

            finishMotion = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, maxSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, nextPosition) < 0.01F)
        {
            finishMotion = true;
        }
    }
}