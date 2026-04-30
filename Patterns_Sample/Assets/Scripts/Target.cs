using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour, IFactoryProduct
{
    private const float TIME_TO_DESTROY = 10F;

    [SerializeField]
    private int maxHP = 1;

    private int currentHP;

    [SerializeField]
    private int scoreAdd = 10;

    private TargetPoolBase pool;

    private bool alreadyReturned;

    public delegate void OnTargetDestroyed(int scoreAdd);

    public static event OnTargetDestroyed onTargetDestroyed;

    public void SetPool(TargetPoolBase targetPool)
    {
        pool = targetPool;
    }

    public void ResetTarget()
    {
        CancelInvoke("ReturnToPool");

        currentHP = maxHP;
        alreadyReturned = false;

        gameObject.SetActive(true);

        Invoke("ReturnToPool", TIME_TO_DESTROY);
    }

    private void OnCollisionEnter(Collision collision)
    {
        int collidedObjectLayer = collision.gameObject.layer;

        if (collidedObjectLayer.Equals(Utils.BulletLayer))
        {
            Pool.Instance.ReturnBullet(collision.gameObject.GetComponent<Bullet>());

            currentHP -= 1;

            if (currentHP <= 0)
            {
                onTargetDestroyed?.Invoke(scoreAdd);
                ReturnToPool();
            }
        }
        else if (collidedObjectLayer.Equals(Utils.PlayerLayer) ||
            collidedObjectLayer.Equals(Utils.KillVolumeLayer))
        {
            Player.Instance.OnPlayerHit?.Invoke();
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (alreadyReturned)
        {
            return;
        }

        alreadyReturned = true;

        CancelInvoke("ReturnToPool");

        if (pool != null)
        {
            pool.ReturnTarget(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}