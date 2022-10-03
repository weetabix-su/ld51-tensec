using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipControl : MonoBehaviour
{
    [Header("Ship Parameters")]
    [Range(0.1f, 20f)] public float maxSpeed = 2f;
    public UnityEvent onShipDestroy;
    protected float currentSpeed;

    [Header("Bullet Parameters")]
    public BulletMoveForward bulletObject;
    public string bulletTag = "Bullet";
    [Range(0.001f, 2f)] public float bulletInterval = 0.125f;
    public float bulletSpawnForwardOffset = 0.675f;
    protected float bulletClock;


    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void OnEnable()
    {
        currentSpeed = 0f;
        bulletClock = 0f;
    }

    public virtual void FixedUpdate()
    {
        rb.AddForce(transform.up * currentSpeed);
    }

    public virtual void Update()
    {
        if (bulletClock < 0f)
            bulletClock = 0f;
        else if (bulletClock > 0f)
            bulletClock -= Time.deltaTime;
    }

    public void FireBullet(UnityAction<Collision2D> onBulletHit = null)
    {
        if (bulletObject == null)
            return;

        if (bulletClock > 0f)
            return;

        BulletMoveForward b = Instantiate(bulletObject, transform.position + (transform.up * bulletSpawnForwardOffset), transform.rotation);
        b.gameObject.tag = bulletTag;
        if (onBulletHit != null)
            b.onHit.AddListener(onBulletHit);

        fireSFX();

        bulletClock = bulletInterval;
    }

    public virtual void DestroyShip()
    {
        onShipDestroy.Invoke();
        Destroy(this.gameObject);
    }

    public virtual void fireSFX()
    {

    }
}
