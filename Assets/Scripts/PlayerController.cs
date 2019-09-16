using System.Collections;
using UnityEngine;

public class PlayerController : MotionController
{
    [SerializeField] private GameObject projectileGO;

    [SerializeField] private float projectileOffset;
    [SerializeField] private float slowSpeed;
    [SerializeField] private float shootingSpeed;

    private bool onTriggerStone = false;
    private bool gunReload = false;
    private float currentSpeed;

    void Update()
    {
        Move();

        if (Input.GetButton("Fire1"))
        {
            if (!gunReload)
            {
                gunReload = true;
                StartCoroutine(FireAndWait());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "StonePart")
        {
            Destroy(gameObject);
            Debug.Log("player destroyed");
        }
        else
        {
            onTriggerStone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        onTriggerStone = false;
    }

    IEnumerator FireAndWait()
    {
        Fire();
        yield return new WaitForSeconds(shootingSpeed);
        gunReload = false;
    }

    private void Fire()
    {
        GameObject newProjectile = projectileGO;
        newProjectile.transform.position = transform.position + projectileOffset * Vector3.up;
        Instantiate(projectileGO);
    }

    protected override void Move()
    {
        Vector3 pos = transform.position;

        currentSpeed = maxSpeed;

        if (onTriggerStone)
        {
            currentSpeed = slowSpeed;
        }

        pos.x += Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        pos.y += Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

        BoundaryClamp(ref pos);

        transform.position = pos;
    }
}
