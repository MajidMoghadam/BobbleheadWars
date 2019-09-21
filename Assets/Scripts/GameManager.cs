using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //used to keep track of the players position
    public GameObject player;
    //keep track of all spawn points
    public GameObject[] spawnPoints;
    //used to spawn new aliens
    public GameObject alien;

    public int maxAliensOnScreen;
    public int totalAliens;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int aliensPerSpawn;

    //used to determine whether to spawn another alien or wait
    private int aliensOnScreen = 0;
    //tracks time between spawns, will randomize this
    private float generatedSpawnTime = 0;
    //milliseconds since last spawnss
    private float currentSpawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnTime += Time.deltaTime;

        if (currentSpawnTime > generatedSpawnTime)
        {
            currentSpawnTime = 0;
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                List<int> previousSpawnLocations = new List<int>();

                if (aliensPerSpawn > spawnPoints.Length)
                {
                    aliensPerSpawn = spawnPoints.Length - 1;
                }

                aliensPerSpawn = (aliensPerSpawn > totalAliens) ? aliensPerSpawn - totalAliens : aliensPerSpawn;

                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    if (aliensOnScreen < maxAliensOnScreen)
                    {
                        aliensOnScreen += 1;

                        // 1
                        int spawnPoint = -1;
                        // 2
                        while (spawnPoint == -1)
                        {
                            // 3
                            int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                            // 4
                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                previousSpawnLocations.Add(randomNumber);
                                spawnPoint = randomNumber;
                            }
                        }

                        GameObject spawnLocation = spawnPoints[spawnPoint];

                        GameObject newAlien = Instantiate(alien) as GameObject;

                        newAlien.transform.position = spawnLocation.transform.position;

                        Alien alienScript = newAlien.GetComponent<Alien>();

                        alienScript.target = player.transform;

                        Vector3 targetRotation = new Vector3(player.transform.position.x,
                            newAlien.transform.position.y, player.transform.position.z);

                        newAlien.transform.LookAt(targetRotation);
                    }
                }

            }

        }
    }
}
