using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LifePickup : ShipControl
{
    [Header("Spin Parameters")]
    public float spin = 2f;
    public float life = 20f;

    GameManager manager;
    Collider2D col;

    float clock;

    public override void OnEnable()
    {
        base.OnEnable();
        manager = GameManager.instance;
        if (manager == null)
            manager = FindObjectOfType<GameManager>();
        col = GetComponent<Collider2D>();
        clock = life;
    }

    public override void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z + (spin * Time.deltaTime));
    }

    public override void Update()
    {
        if (clock > 0f)
            clock -= Time.deltaTime;
        else
            DestroyShip();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            manager.AddLife();
            DestroyShip();
        }
    }
}
