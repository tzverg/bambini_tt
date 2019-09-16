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

#if UNITY_ANDROID
    private Vector2 touchDirection;
#endif

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
            UIController.gameOver = true;
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

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        pos.x += Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        pos.y += Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
#endif
#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchDirection = Input.GetTouch(0).deltaPosition.normalized;
            pos.x += touchDirection.x * currentSpeed * Time.deltaTime;
            pos.y += touchDirection.y * currentSpeed * Time.deltaTime;
        }
#endif

        BoundaryClamp(ref pos);

        transform.position = pos;
    }
}
