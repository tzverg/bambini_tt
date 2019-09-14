using UnityEngine;

public abstract class MotionController : MonoBehaviour
{
    [SerializeField] public float maxSpeed;

    protected virtual void Move() { }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected void BoundaryClamp(ref Vector3 targetPos)
    {
        float clampValue = Camera.main.orthographicSize;

        targetPos.x = Mathf.Clamp(targetPos.x, -clampValue, clampValue);
        targetPos.y = Mathf.Clamp(targetPos.y, -clampValue + 1, clampValue - 1);
    }
}