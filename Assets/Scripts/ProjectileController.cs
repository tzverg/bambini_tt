using UnityEngine;

public class ProjectileController : MotionController
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        Destroy(gameObject);
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
