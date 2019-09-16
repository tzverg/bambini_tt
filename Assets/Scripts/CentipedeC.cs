using System.Collections.Generic;
using UnityEngine;

public class CentipedeC : MotionController
{
    [SerializeField] private Vector3 motionStepY;

    private bool directionLeft = true;
    private bool directionBottom = false;

    private Vector3 VerticalPos;

    public int centipedeLenght;
    public float bodyOffset;

    public GameObject centipedeBodyGO;
    public GameObject centipedeHeadGO;

    public List<GameObject> centipedeBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (centipedeBody.Count > 0)
            {
                gameObject.transform.position = centipedeBody[0].transform.position;
                RemoveBodyPart(0);
            }

            UIController.AddSoresNum(50);
        }
        else if (collision.gameObject.CompareTag("StonePart"))
        {
            ChangeDirection(true);
        }
    }

    private void RemoveBodyPart(int bodyPartID)
    {
        Destroy(centipedeBody[bodyPartID]);
        centipedeBody.Remove(centipedeBody[bodyPartID]);

        foreach (GameObject target in centipedeBody)
        {
            CentipedeBodyC targetCentipedBody = target.GetComponent<CentipedeBodyC>();
            if (targetCentipedBody != null)
            {
                targetCentipedBody.bodyID = centipedeBody.IndexOf(target);
            }
        }
    }

    public void HitProjectileOn(int bodyPartID)
    {
        if (bodyPartID < centipedeBody.Count)
        {
            GameObject newHeadGO = Instantiate(centipedeHeadGO, centipedeBody[bodyPartID].transform.position, transform.rotation);
            CentipedeC newHead = newHeadGO.GetComponent<CentipedeC>();
            if (newHead != null)
            {
                RemoveBodyPart(bodyPartID);

                SetParams(ref newHead, bodyPartID);

                for (int cnt = 0; cnt < newHead.centipedeLenght; cnt++)
                {
                    CentipedeBodyC targetCentipedBody = centipedeBody[bodyPartID + cnt].GetComponent<CentipedeBodyC>();
                    if (targetCentipedBody != null)
                    {
                        newHead.centipedeBody.Add(centipedeBody[bodyPartID + cnt]);
                        targetCentipedBody.head = newHead;
                        targetCentipedBody.bodyID = cnt;
                    }
                }

                centipedeBody.RemoveRange(bodyPartID, newHead.centipedeLenght);

                centipedeLenght = centipedeBody.Count;
                newHead.ChangeDirection(true);

                GameC.enemyList.Add(newHead.gameObject);
            }
        }
    }

    private void SetParams(ref CentipedeC newHead, int bodyPartID)
    {
        newHead.directionLeft = directionLeft;
        newHead.maxSpeed = maxSpeed;
        newHead.bodyOffset = bodyOffset;
        newHead.motionStepY = motionStepY;
        newHead.centipedeBodyGO = centipedeBodyGO;
        newHead.centipedeHeadGO = centipedeHeadGO;

        newHead.centipedeLenght = centipedeBody.Count - bodyPartID;
        newHead.centipedeBody = new List<GameObject>();
    }

    private void ChangeDirection(bool toBottom)
    {
        directionBottom = toBottom;

        if (directionBottom)
        {
            VerticalPos = transform.position + motionStepY;
        }
        else
        {
            directionLeft = !directionLeft;
        }

        RotateHead();
    }

    private void RotateHead()
    {
        if (!directionBottom)
        {
            if (directionLeft)
            {
                transform.rotation = Quaternion.Euler(0F, 0F, -90F);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0F, 0F, 90F);
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0F, 0F, 0F);
        }
    }

    private void BoundaryCheck(ref Vector3 targetPos)
    {
        float maxValue = Camera.main.orthographicSize;

        if (!directionBottom)
        {
            if ((targetPos.x <= -maxValue) || (targetPos.x >= maxValue))
            {
                BoundaryClamp(ref targetPos);
                ChangeDirection(true);
            }
        }
        else
        {
            if ((targetPos.y <= VerticalPos.y))
            {
                targetPos = VerticalPos;
                ChangeDirection(false);
            }

            if (targetPos.y < -maxValue)
            {
                UIController.gameOver = true;
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

        MoveBodyParts();
    }

    private Vector3 GetBPNextPosition(int cnt)
    {
        if (cnt == 0)
        {
            return transform.position;
        }
        else
        {
            return centipedeBody[cnt - 1].transform.position;
        }
    }

    private void MoveBodyParts()
    {
        for (int cnt = 0; cnt < centipedeBody.Count; cnt++)
        {
            Vector3 nextPos = GetBPNextPosition(cnt);
            Vector3 pos = centipedeBody[cnt].transform.position;
            Vector3 direction = nextPos - pos;

            pos = Vector3.MoveTowards(pos, nextPos, (maxSpeed + 0.1F) * Time.deltaTime);

            if (Vector3.Distance(nextPos, centipedeBody[cnt].transform.position) > bodyOffset)
            {
                centipedeBody[cnt].transform.position = pos;
            }

            centipedeBody[cnt].transform.up = direction;
        }
    }
}
