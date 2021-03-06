using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    void Shoot();
}

public class AdvancedBulletScript : SimpleBullet, IShootable
{
    //ObjectQueue queue;

    public enum BulletTypes
    {
        SPREAD,
        DOUBLE,
        RICOCHET,
        MEGA,
        STUN,
    };

    public BulletTypes bulletType;

    // Start is called before the first frame update
    void Start()
    {
        //queue = ObjectQueue.Instance;
    }

    protected void Update() {
        base.Update();
    }

    void IShootable.Shoot()
    {
        switch (bulletType)
        {
            case BulletTypes.DOUBLE:
                DoubleAction();
                break;
            case BulletTypes.SPREAD:
                SpreadAction();
                break;
            default:
                break;
        }
        
    }

    public float spreadOffsetAngle = 15;
    public float spreadStartX = .25f;
    public float spreadStartY = -.1f;
    void SpreadAction()
    {
        GameObject bullet = ObjectQueue.Instance.SpawnFromPool("PlayerBullet", transform.position, transform.rotation * Quaternion.Euler(0, 0, -spreadOffsetAngle));
        bullet.transform.Translate(spreadStartX, spreadStartY, 0);
        bullet = ObjectQueue.Instance.SpawnFromPool("PlayerBullet", transform.position, transform.rotation * Quaternion.Euler(0, 0, spreadOffsetAngle));
        bullet.transform.Translate(-spreadStartX, spreadStartY, 0);
    }

    public float doubleStartPoint = 0.1f;
    void DoubleAction()
    {
        GameObject bullet = ObjectQueue.Instance.SpawnFromPool("PlayerBullet", transform.position, transform.rotation);
        bullet.transform.Translate(doubleStartPoint, 0, 0);
        bullet = ObjectQueue.Instance.SpawnFromPool("PlayerBullet", transform.position, transform.rotation);
        bullet.transform.Translate(-doubleStartPoint, 0, 0);
    }

    public float ricochetAmount = 0;

    void RicochetAction() {
        GameObject bullet = ObjectQueue.Instance.SpawnFromPool("PlayerBullet", transform.position, transform.rotation);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Environment")) {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, other.transform.position, (1 << LayerMask.NameToLayer("Environment")));
        }
    }
}
