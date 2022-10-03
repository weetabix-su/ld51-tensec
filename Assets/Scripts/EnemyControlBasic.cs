using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyControlBasic : ShipControl
{
    [Header("Player Proximity Values")]
    public float distanceBeforeShooting = 3f;

    PlayerControl target;
    Collider2D col;

    GameManager manager;

    public override void OnEnable()
    {
        base.OnEnable();

        if (target == null)
            target = FindObjectOfType<PlayerControl>();

        col = GetComponent<Collider2D>();

        manager = GameManager.instance;
        if (manager == null)
            manager = FindObjectOfType<GameManager>();
    }

    public override void FixedUpdate()
    {
        transform.position += (new Vector3(transform.up.x, transform.up.y, 0f) * maxSpeed * Time.deltaTime);
    }

    public override void Update()
    {
        base.Update();

        transform.up = target.transform.position - transform.position;
        if (Vector2.Distance(transform.position, target.transform.position) <= distanceBeforeShooting) 
        {
            FireBullet((col) =>
            {
                if (col.gameObject == target.gameObject)
                {
                    manager.ReduceLife();
                }
            });
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target.gameObject)
        {
            manager.ReduceLife();
            DestroyShip();
        }
    }

    public override void DestroyShip()
    {
        manager.playSFXEnemyDie();

        base.DestroyShip();
    }

    public override void fireSFX()
    {
        manager.playSFXEnemyLaser();
    }
}
