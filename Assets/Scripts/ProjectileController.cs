using UnityEngine;

public class ProjectileController : MotionController
{
    private GameC gameC;

    private void Start()
    {
        gameC = FindObjectOfType<GameC>();

        if (!gameC)
        {
            Debug.LogError("GameC object not found");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.CompareTag("CentipedeHead"))
        {
            CentipedeC targetHead = collision.gameObject.GetComponent<CentipedeC>();
            if (targetHead != null)
            {
                if (targetHead.centipedeBody.Count == 0)
                {
                    Destroy(collision.gameObject);
                    GameC.enemyList.Remove(collision.gameObject);
                }
            }
        }
        else
        {
            Destroy(collision.gameObject);
            GameC.enemyList.Remove(collision.gameObject);
        }

        if (gameC != null)
        {
            gameC.CheckEnemyList();
        }
    }

    override protected void Move()
    {
        if (transform.position.y > Camera.main.orthographicSize + 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 pos = transform.position;
            pos.y += maxSpeed * Time.deltaTime;
            transform.position = pos;
        }
    }
}
