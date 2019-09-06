using UnityEngine;

public class SpiderController : MotionController
{
    [SerializeField] private float maxOffsetFromPlayer;
    [SerializeField] private float offsetFreeMotion;

    [SerializeField] private Transform playerTR;

    private Vector3 nextPosition;
    private float randomDistance;
    private float curOffsetFromPlayer;
    private bool finishMotion = true;

    // Update is called once per frame
    void Update()
    {
        Move();
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

        if (randomSeed > 0.5F)
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
        if (finishMotion == true)
        {
            nextPosition = transform.position;

            nextPosition.x = Random.Range(-offsetFreeMotion, offsetFreeMotion);
            nextPosition.y = Random.Range(-offsetFreeMotion, offsetFreeMotion);

            BoundaryClamp(ref nextPosition);

            Debug.Log("nextPosition: " + nextPosition.ToString());

            finishMotion = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, maxSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, nextPosition) < 0.01F)
        {
            finishMotion = true;
        }

        Debug.Log("Distance: " + Vector3.Distance(transform.position, nextPosition));
    }
}