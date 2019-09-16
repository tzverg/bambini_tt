using System.Collections.Generic;
using UnityEngine;

public class StoneC : MonoBehaviour
{
    private const float maxPosOffset = 0.2F;
    private List<Transform> childrenTRL;

    public int mapIDX;
    public int mapIDY;

    void Start()
    {
        childrenTRL = new List<Transform>();
        for (int cnt = 0; cnt < transform.childCount; cnt++)
        {
            Transform childTR = transform.GetChild(cnt);
            if (childTR.gameObject.CompareTag("StonePart"))
            {
                SetRandomPR(ref childTR);
                childrenTRL.Add(childTR);
            }
        }
    }

    private void SetRandomPR(ref Transform targetTR)
    {
        Vector3 posOffset = new Vector3(Random.Range(-maxPosOffset, maxPosOffset), Random.Range(-maxPosOffset, maxPosOffset), 0F);
        Quaternion targetRot = Quaternion.Euler(0F, 0F, Random.Range(0F, 359F));

        targetTR.position += posOffset;
        targetTR.rotation = targetRot;
    }
}
