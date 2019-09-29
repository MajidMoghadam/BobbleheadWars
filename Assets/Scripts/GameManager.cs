using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //used to keep track of the players position
    //set in the inspector
    public GameObject player;
    //keep track of all spawn points
    //set in the inspector
    public GameObject[] spawnPoints;
    //used to spawn new aliens
    //set in inspector
    public GameObject alien;
    public GameObject upgradePrefab;

    public Gun gun;
    public float upgradeMaxTimeSpawn = 7.5f;
    private bool spawnedUpgrade = false;
    private float actualUpgradeTime = 0;
    private float currentUpgradeTime = 0;

    //parameters to control number of aliens 
    //on screen at any one time
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
        actualUpgradeTime = Random.Range(upgradeMaxTimeSpawn - 3.0f, upgradeMaxTimeSpawn);
        actualUpgradeTime = Mathf.Abs(actualUpgradeTime);
    }

    // Update is called once per frame (almost like a loop, keeps repeating)
    void Update()
    {
        currentUpgradeTime += Time.deltaTime;
        if (currentUpgradeTime > actualUpgradeTime)
        {
            // 1
            if (!spawnedUpgrade)
            {
                // 2
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                // 3
                GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
                Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
                upgradeScript.gun = gun;
                upgrade.transform.position = spawnLocation.transform.position;
                // 4
                //accumulates time that's passed between each frame
                currentSpawnTime += Time.deltaTime;
                spawnedUpgrade = true;

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
            }
        }

        //whenever a certain amount of time has passed do the following
        if (currentSpawnTime > generatedSpawnTime)
        {
            //reset currentSpawnTime to zero to start accumalating
            //time for next spawn
            currentSpawnTime = 0;

            //random time for next spawn
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            //preventative code to stop spawning if maximum number of spawns reached
            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                //a list is like a dynamic array that can grow and shrink 
                //use this to keep track of where you spawn aliens so you do
                //not spawn more than one alien from the same spot each wave
                List<int> previousSpawnLocations = new List<int>();


                //This limits the number of aliens you can spawn by the number of spawn points.
                if (aliensPerSpawn > spawnPoints.Length)
                {
                    aliensPerSpawn = spawnPoints.Length - 1;
                }

                //fancy if-else statement called the ternary operator
                //This is another chunk of preventative code. If aliensPerSpawn exceeds the maximum,
                //then the amount of spawns will reduce.
                aliensPerSpawn = (aliensPerSpawn > totalAliens) ? aliensPerSpawn - totalAliens : aliensPerSpawn;
                /*
                 could have written the above code like this:

                if(aliensPerSpawn > totalAliens){
                    aliensPerSpawn = aliensPerSpawn - totalAliens;
                }else{
                    aliensPerSpawn = aliensPerSpawn;
                }
                 */


                //the actual spawning code inside the loop
                //loops once for each spawned alien
                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    //only spawn if we haven't exceeded maximum aliens on screen
                    if (aliensOnScreen < maxAliensOnScreen)
                    {
                        //since we will get a new alien spawned
                        //increment aliens on screen by one
                        aliensOnScreen += 1;

                        // need to find the spawning point (index of List where it will spawn)
                        //randomly generate an index, check if its populated previously
                        //if not then thats your spawnpoint, otherwise try again
                        int spawnPoint = -1;
                        while (spawnPoint == -1)
                        {
                            int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                previousSpawnLocations.Add(randomNumber);
                                spawnPoint = randomNumber;
                            }
                        }
                        //Create(instantiate) an alien object from the prefab
                        //set the alien's position to the spawnpoint location you identified above
                        //attach an alien script to the alien object (just like we did in inspector)
                        //set the target property of the script to the player (just like we did 
                        //in inspector) 
                        GameObject newAlien = Instantiate(alien) as GameObject;
                        GameObject spawnLocation = spawnPoints[spawnPoint];
                        newAlien.transform.position = spawnLocation.transform.position;
                        Alien alienScript = newAlien.GetComponent<Alien>(); //gets a script object!!
                        alienScript.target = player.transform;              //script target needs to be set


                        //Alien's target is the player's x and z coordinate (it's own y, to stay level)
                        //the LookAt makes the alien immediately turn towards target(player)
                        //it moves towards the player due to its own walk animation
                        Vector3 targetRotation = new Vector3(player.transform.position.x,
                            newAlien.transform.position.y, player.transform.position.z);
                        newAlien.transform.LookAt(targetRotation);

                        alienScript.OnDestroy.AddListener(AlienDestroyed);
                    }
                }

            }

        }
    }

    public void AlienDestroyed()
    {
        aliensOnScreen -= 1;
        totalAliens -= 1;
    }
}
