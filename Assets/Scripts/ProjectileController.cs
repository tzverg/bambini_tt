using UnityEngine;

public class ProjectileController : MotionController
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision " + gameObject.name + " with " + collision.gameObject.name);
        Destroy(gameObject);

        if (!collision.gameObject.CompareTag("CentipedeHead"))
        {
            Destroy(collision.gameObject);
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
