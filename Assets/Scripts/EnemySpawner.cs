using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Parameters")]
    public GameObject[] spawnables;
    public Vector2 spawnArea = Vector2.one * 20f;

    GameManager manager;

    private void Awake()
    {
        manager = GetComponent<GameManager>();
        manager.onClockTick.AddListener(Spawn);
    }

    public void Spawn()
    {
        if (spawnables.Length < 1)
            return;

        for (int i = 0; i < manager.fib; i++)
        {
            Instantiate(spawnables[Random.Range(0, spawnables.Length)],
                transform.position + new Vector3(Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f), Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f), transform.position.z),
                Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f)));
        }
    }
}
