using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerControl : ShipControl
{
    [Header("Bullet Cooldown Values")]
    [Range(0.001f, 0.25f)] public float bulletCooldownIncrement = 0.0078125f;
    public float passiveCooldownMultiplier = 0.25f;
    [Range(0.1f, 10f)] public float cooldownInterval = 10f;
    float cooldownValue;
    float cooldownClock;
    bool isFullCooldown;
    Vector2 stickValue;
    bool stickUsed;

    GameManager manager;

    public override void OnEnable()
    {
        base.OnEnable();

        if (manager == null)
            manager = GameManager.instance;
        if (manager == null)
            manager = FindObjectOfType<GameManager>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public override void Update()
    {
        base.Update();

        if (stickUsed)
        {
            transform.rotation = Quaternion.Euler(Vector3.forward * Mathf.Atan2(-stickValue.x, stickValue.y) * Mathf.Rad2Deg);
        }
        currentSpeed = stickUsed ? maxSpeed : 0f;

        if (!isFullCooldown)
        {
            if (cooldownValue <= 1f)
            {
                if (stickUsed && bulletClock <= 0f)
                {
                    cooldownValue += bulletCooldownIncrement;
                    FireBullet((col) =>
                    {
                        if (col.gameObject.tag == "Enemy")
                        {
                            col.gameObject.GetComponent<ShipControl>().DestroyShip();
                            manager.AddPoint();
                        }
                    });
                }
                if (cooldownValue > 0f)
                {
                    cooldownValue -= bulletCooldownIncrement * passiveCooldownMultiplier;
                }
                else
                    cooldownValue = 0f;
            }
            else
                StartCoroutine(BulletCooldown());
        }

        manager.SetCooldownBarValue(cooldownValue);
    }

    public void OnMove(InputValue value)
    {
        stickValue = value.Get<Vector2>();
        stickUsed = stickValue != Vector2.zero;
    }

    IEnumerator BulletCooldown()
    {
        isFullCooldown = true;
        while (cooldownValue > 0f)
        {
            cooldownValue -= Time.deltaTime / 10f;
            yield return null;
        }
        isFullCooldown = false;
    }

    public override void DestroyShip()
    {
        manager.playSFXPlayerDie();

        base.DestroyShip();
    }

    public override void fireSFX()
    {
        manager.playSFXPlayerLaser();
    }
}
