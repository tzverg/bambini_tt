using UnityEngine;

public abstract class MotionController : MonoBehaviour
{
    [SerializeField] protected float maxSpeed;

    protected virtual void Move() { }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected void BoundaryClamp(ref Vector3 targetPos)
    {
        float clampValue = Camera.main.orthographicSize;
        float offsetY = 0.5F;

        targetPos.x = Mathf.Clamp(targetPos.x, -clampValue, clampValue);
        targetPos.y = Mathf.Clamp(targetPos.y, -clampValue + offsetY, clampValue - offsetY);
    }
}