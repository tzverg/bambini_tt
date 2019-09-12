using System.Collections.Generic;
using UnityEngine;

public class CentipedeHeadC : MotionController
{
    [SerializeField] private GameObject bodySectionGO;
    [SerializeField] private Vector3 bodyOffset;
    [SerializeField] private Vector3 motionStepY;

    [SerializeField] private int centipedeLenght;

    private bool directionLeft = true;
    private bool directionBottom = false;

    private Vector3 VerticalPos;

    private void Start()
    {
        Stack<GameObject> centipedeBody = new Stack<GameObject>();

        //for (int cnt = 0; cnt < centipedeLenght; cnt++)
        //{
        //    GameObject bodySection = Instantiate(bodySectionGO);
        //    bodySection.transform.position = transform.position + bodyOffset * (cnt + 1);
        //    centipedeBody.Push(bodySection);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    private void ChangeDirection()
    {
        if (directionLeft)
        {
            transform.Rotate(Vector3.forward, 90F);
        }
        else
        {
            transform.Rotate(Vector3.forward, -90F);
        }

        if (!directionBottom)
        {
            directionLeft = !directionLeft;
        }
    }

    private void BoundaryCheck(ref Vector3 targetPos)
    {
        float maxValue = Camera.main.orthographicSize;

        if (!directionBottom)
        {
            if ((targetPos.x <= -maxValue) || (targetPos.x >= maxValue))
            {
                directionBottom = true;
                BoundaryClamp(ref targetPos);
                VerticalPos = targetPos + motionStepY;
                ChangeDirection();
            }
        }
        else
        {
            if ((targetPos.y <= VerticalPos.y))
            {
                directionBottom = false;
                targetPos = VerticalPos;
                ChangeDirection();
            }
        }
    }

    protected override void Move()
    {
        Vector3 pos = transform.position;

        if (directionBottom)
        {
            pos.y -= maxSpeed * Time.deltaTime;
        }
        else
        {
            if (directionLeft)
            {
                pos.x -= maxSpeed * Time.deltaTime;
            }
            else
            {
                pos.x += maxSpeed * Time.deltaTime;
            }
        }

        BoundaryCheck(ref pos);

        transform.position = pos;
    }
}
