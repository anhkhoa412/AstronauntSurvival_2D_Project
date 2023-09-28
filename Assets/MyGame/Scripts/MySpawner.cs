using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySpawner : MonoBehaviour
{
    // Initial and maximum time between spawns
    public float initialTimeBetweenSpawns = 2f;
    public float maxTimeBetweenSpawns = 5f;
    private float timeBetweenSpawns;
    private float nextSpawnTime;

    // Timer variables for spawn rate adjustment
    private float gameTimer = 0f;
    public float timeToIncreaseSpawn = 10f;

    // Minimum and maximum spawn rates
    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;

    // Number of spawns to increase after a certain time
    public int spawnsToIncrease = 1;

    // Maximum number of spawns
    public int maxSpawns = 10;

    // Time after which to increase the number of spawns
    public float timeToIncreaseSpawns = 20f;

    // Duration of the announcement effect and its prefab
    public float announcementDuration = 3.0f;
    public GameObject announcementObjectPrefab;

    // Struct to define spawnable objects and their spawn chances
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;

        [Range(0f, 1f)]
        public float spawnChance;
    }

    // Array of spawnable objects
    public SpawnableObject[] spawnables;

    // Array of spawn points for objects
    public Transform[] spawnPoints;

    // Initialization on game start
    private void Start()
    {
        timeBetweenSpawns = initialTimeBetweenSpawns;
        nextSpawnTime = Time.time + timeBetweenSpawns;
        spawnPoints = gameObject.GetComponentsInChildren<Transform>();
    }

    // Called when the script is enabled
    private void OnEnable()
    {
        nextSpawnTime = Time.time + timeBetweenSpawns;
    }

    // Called when the script is disabled
    private void OnDisable()
    {
        CancelInvoke();
    }

    // Update is called once per frame
    private void Update()
    {
        gameTimer += Time.deltaTime;

        // Check if it's time to increase the number of spawns
        if (gameTimer >= timeToIncreaseSpawns)
        {
            timeToIncreaseSpawns += timeToIncreaseSpawns; // Increase the time for the next spawn rate increase
            IncreaseSpawns();
        }

        // Spawn objects based on time and spawn rate
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + timeBetweenSpawns;
            StartCoroutine(Spawn());
        }
    }

    // Coroutine for spawning objects
    private IEnumerator Spawn()
    {
        for (int i = 0; i < spawnsToIncrease; i++)
        {
            float spawnChance = Random.value;

            // Spawn objects based on spawn chances
            foreach (var spawnable in spawnables)
            {
                if (spawnChance < spawnable.spawnChance)
                {
                    Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    Instantiate(announcementObjectPrefab, randomSpawnPoint.position, Quaternion.identity);
                    yield return new WaitForSeconds(announcementDuration + 0.4f); // Add a delay after announcement
                    GameObject spawnedObject = Instantiate(spawnable.prefab, randomSpawnPoint.position, Quaternion.identity);
                    spawnedObject.transform.parent = transform; // Parent the spawned object to the spawner
                    break;
                }

                spawnChance -= spawnable.spawnChance;
            }
        }
    }

    // Increase the number of spawns
    private void IncreaseSpawns()
    {
        // Increase the spawns up to the maximum value
        if (spawnsToIncrease < maxSpawns)
        {
            spawnsToIncrease++;
        }

        // Optionally, you can also adjust other parameters like spawn rate or announcement duration here.
    }
}
