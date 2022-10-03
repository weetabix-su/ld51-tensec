using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class BulletMoveForward : MonoBehaviour
{
    public class HitEvent : UnityEvent<Collision2D>
    {

    }

    public float speed = 2.25f;

    Collider2D col;
    float ttl = 10f;

    public HitEvent onHit = new HitEvent();

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        if (ttl > 0f)
            ttl -= Time.deltaTime;
        else
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onHit.Invoke(collision);

        if (collision.gameObject.GetComponent<BulletTargetObject>() != null)
        {
            collision.gameObject.GetComponent<BulletTargetObject>().OnBulletHit();
        }
        if (!CompareTag(collision.gameObject.tag))
        {
            onHit.RemoveAllListeners();
            Destroy(this.gameObject);
        }
    }
}
