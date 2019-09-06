using UnityEngine;

public class PlayerController : MotionController
{
    [SerializeField] private float projectileOffset;

    [SerializeField] private GameObject projectileGO;

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
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

        pos.x += Input.GetAxis("Horizontal") * maxSpeed * Time.deltaTime;
        pos.y += Input.GetAxis("Vertical") * maxSpeed * Time.deltaTime;

        BoundaryClamp(ref pos);

        transform.position = pos;
    }
}
