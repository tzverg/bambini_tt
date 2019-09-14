using UnityEngine;

public class CentipedeBodyC : MonoBehaviour
{
    public int bodyID;
    public CentipedeC head;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            head.centipedeBody.Remove(gameObject);
            head.HitProjectileOn(bodyID);
        }
    }
}
