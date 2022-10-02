using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Spawner")]
    [Tooltip("How often to spawn power-ups")]
    [SerializeField]
    private float repeatFactor;
    [SerializeField]
    private GameObject[] powerUps;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnPowerUps), repeatFactor, repeatFactor);
    }

    void SpawnPowerUps()
    {
        GameObject[] activeExplosions = GameObject.FindGameObjectsWithTag("Explosion");
        foreach (GameObject explosion in activeExplosions)
        {
            Destroy(explosion);
        }

        GameObject[] activeGems = GameObject.FindGameObjectsWithTag("Gem");
        foreach(GameObject gem in activeGems)
        {
            Destroy(gem);
        }

        float spawnX = Random.Range(-4.5f, 4.5f);
        float spawnY = 1.5f;
        float spawnZ = Random.Range(-9.0f, -2.0f);
        int whichOne = Random.Range(0, powerUps.Length);

        Instantiate(powerUps[whichOne], new Vector3(spawnX, spawnY, spawnZ), powerUps[whichOne].transform.rotation);
        Instantiate(powerUps[whichOne], new Vector3(-spawnX, spawnY, -spawnZ), powerUps[whichOne].transform.rotation);

    }
}
