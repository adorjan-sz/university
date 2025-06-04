using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static SpawnLocation;
using static UnityEngine.UI.GridLayoutGroup;
public enum PlayerPerks
{
    moreDamage,
    moreHealth,
    moreSpeed,
    upgradeSword,
    moreArrows
}
public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance { get; private set; }
    public event Action<List<PlayerPerks>> ShowRemaining;
    private List<Enemy> enemies = new List<Enemy>();
    private List<PlayerPerks> AllPerk = new List<PlayerPerks>();

    [Header("Spawn Settings")]
    [SerializeField]
    private Terrain terrain;
    private float xBounds;
    private float zBounds;
    private Vector3 position;
    private Vector3 size;
    [SerializeField]
    [Range(10.0f, 100.0f)]
    private float requiredSpawnDistance;  //so that the player doesn't see the enemies spawn.
    [SerializeField]
    [Range(1, 100)]
    private float timeBetweenSpawn;       //the time between spawning hordes.
    [SerializeField]
    [Range(10, 2000)]
    private int maxEnemyCount;            //the maximum number of enemies that can be on the map.  
    [SerializeField]
    private int enemiesInHorde;           //the number of enemies a the spawner will spawn.
    [SerializeField]
    private bool debugMode;               //draw the spawn and camera lines on the world space
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float spawnPadding;
    Vector3[] camBounds = new Vector3[4]; //The initial world point corners which the camera sees
    Vector3[] curBounds = new Vector3[4]; //offset with the current player position
    Vector3[] spawnBounds;                // The bounds of the spawning area

    [Header("Enemy Settings")]
    public Player player;
    private Camera playercam;
    public GameObject Skeleton;
    public GameObject Barbar;

    public float PlayerDamage = 1f;
    public int PlayerHealth = 1;
    public int arrows = 1;
    public float PlayerSpeed = 1f;


    void Awake()
    {
        //Debug.Log(terrain.terrainData.size);
        position = terrain.transform.position;
        size     = terrain.terrainData.size;
        xBounds  = position.x + size.x;
        //Debug.Log("az xbounds merete: " + xBounds);
        //Debug.Log("y pozi: " + position.z);
        zBounds = position.z + size.z;
        
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        player = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        playercam = Camera.main;
        player.LevelUp += OnPlayerLevelUp;

    }

    private void Start()
    {
        calculateCameraPositions();
        StartCoroutine(StartBattle());
    }
    void OnPlayerLevelUp(List<PlayerPerks> perks)
    {
        ScaleHorde();

        List<PlayerPerks> perksList = new List<PlayerPerks>();
        PopulatePerksList(perks, perksList);

        ShowRemaining.Invoke(perksList);
    }

    private void ScaleHorde()
    {
        maxEnemyCount = (int)Math.Round(maxEnemyCount * 1.5);
        enemiesInHorde = (int)Math.Round(enemiesInHorde * 1.5);
    }

    private void PopulatePerksList(List<PlayerPerks> perks, List<PlayerPerks> perksList)
    {
        if (PlayerDamage < 3)
        {
            perksList.Add(PlayerPerks.moreDamage);
        }
        if (PlayerHealth < 3)
        {
            perksList.Add(PlayerPerks.moreHealth);
        }
        if (PlayerSpeed < 3)
        {
            perksList.Add(PlayerPerks.moreSpeed);
        }
        if (arrows < 5)
        {
            perksList.Add(PlayerPerks.moreArrows);
        }
    }

    public void Update()
    {
        
        if (debugMode) {
            for (int i = 0; i < curBounds.Length; ++i)
            {
                Debug.DrawLine(curBounds[i], curBounds[(i + 1) % curBounds.Length], UnityEngine.Color.green);
            }
            for (int i = 0; i < spawnBounds.Length; ++i)
            {
                Debug.DrawLine(spawnBounds[i], spawnBounds[(i + 1) % spawnBounds.Length], UnityEngine.Color.red);
            }
        }
    }

    public void AddPerk(PlayerPerks perk)
    {

        //ezt meghivjuk a UIban amikor a játékos választ egy perk-et
        switch (perk)
        {
            case PlayerPerks.moreDamage:
                PlayerDamage++;
                break;
            case PlayerPerks.moreHealth:
                PlayerHealth++;
                break;
            case PlayerPerks.moreSpeed:
                PlayerSpeed++;
                player.speed = 12 * PlayerSpeed;
                break;
            case PlayerPerks.upgradeSword:
                player.UpgradeSword();
                player.perks.Add(perk);
                break;
            case PlayerPerks.moreArrows:
                arrows++;
                player.perks.Add(perk);
                break;
        }
        Time.timeScale = 1;

    }
    public List<Enemy> GetEnemies()
    {
        return enemies;
    }

    public Player GetPlayer()
    {
        return player;
    } 

    public void RegisterEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public void SpawnEnemy()
    {
        bool type = UnityEngine.Random.Range(0, 2) == 1;
        GameObject obj;
        obj = InstanciateObject(type);
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.player = player.transform;
        enemy.tag = "Enemy";
        RegisterEnemy(enemy);
    }

    private GameObject InstanciateObject(bool type)
    {
        GameObject obj;
        if (type)
        {
            obj = Instantiate(Skeleton, GetRandomPositionInbounds(), Quaternion.identity);
        }
        else
        {
            obj = Instantiate(Barbar, GetRandomPositionInbounds(), Quaternion.identity);
        }

        return obj;
    }

    public void SpawnHorde() {
        for (int i = 0; i < enemiesInHorde; ++i) {
            SpawnEnemy();
        }
    }

    private Vector3 GetRandomPositionInbounds() {

        //SpawnLocation.GetRandomDirection()
        //jobb felso curBounds[2]
        //bal felso curBounds[3]
        //bal also curBounds[0]
        //jobb also curbounds[1]
        float topx   =   UnityEngine.Random.Range(Mathf.Max(curBounds[3].x, spawnBounds[0].x), Mathf.Min(curBounds[2].x, spawnBounds[1].x));
        float westz  =  UnityEngine.Random.Range(Mathf.Min(curBounds[3].z, spawnBounds[0].z), Mathf.Max(curBounds[0].z, spawnBounds[3].z));
        float eastz  =  UnityEngine.Random.Range(Mathf.Min(curBounds[2].z, spawnBounds[1].z), Mathf.Max(curBounds[1].z, spawnBounds[2].z));
        float southx = UnityEngine.Random.Range(Mathf.Max(curBounds[0].x, spawnBounds[3].x), Mathf.Min(curBounds[1].x, spawnBounds[2].x));
        switch (GetAvailableDirection())
        {
            case SpawnLocation.SpawnDirection.NORTH: //north
                return new Vector3(topx, 0, curBounds[2].z + requiredSpawnDistance);
            case SpawnLocation.SpawnDirection.WEST: //west
                return new Vector3(curBounds[3].x - requiredSpawnDistance, 0, westz);
            case SpawnLocation.SpawnDirection.EAST: //east
                return new Vector3(curBounds[2].x + requiredSpawnDistance, 0, eastz);
            case SpawnLocation.SpawnDirection.SOUTH: //south
                return new Vector3(southx, 0, curBounds[0].z - requiredSpawnDistance);
            default: //other just in case
                return new Vector3(UnityEngine.Random.Range(position.x, xBounds), 0, UnityEngine.Random.Range(position.z, zBounds));
        }
    }
    IEnumerator StartBattle()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawn);
            updateSpawnLocation();

            if (enemies.Count + enemiesInHorde <= maxEnemyCount) {
     
                SpawnHorde();
            }
        }
    }

    private void updateSpawnLocation() {
        Vector3 cameraOffset = player.transform.position;

        cameraOffset.y = 0;
        for (int i = 0; i < curBounds.Length; ++i)
        {
            //Debug.DrawLine(camBounds[i] + cameraOffset, camBounds[(i + 1) % camBounds.Length] + cameraOffset, UnityEngine.Color.green);
            curBounds[i].x = camBounds[i].x + cameraOffset.x;
            curBounds[i].z = camBounds[i].z + cameraOffset.z;
        }
    }

    private SpawnDirection GetAvailableDirection() {
        List<SpawnDirection> dirs = new List<SpawnDirection>();
        //jobb felső sarkot nézi
        //string debugprint = "";
        if (spawnBounds[1].z - curBounds[2].z > requiredSpawnDistance)
        {
            dirs.Add(SpawnDirection.NORTH);
            //debugprint += "NORTH";
        }
        //west
        if (spawnBounds[0].x - curBounds[3].x <  - requiredSpawnDistance) {
            Debug.Log(spawnBounds[0].x - curBounds[3].x);
            dirs.Add(SpawnDirection.WEST);
            //debugprint += " WEST";
        }
        if (spawnBounds[1].x - curBounds[2].x > requiredSpawnDistance) {
            dirs.Add(SpawnDirection.EAST);
            //debugprint += " EAST";
        }
        if (spawnBounds[3].z - curBounds[0].z < -requiredSpawnDistance) {

            dirs.Add(SpawnDirection.SOUTH);
            //debugprint += " SOUTH";
        }

        if (dirs.Any()) {
            //Debug.Log(debugprint.ToString());
            return dirs[UnityEngine.Random.Range(0, dirs.Count)];
        }
        
        return SpawnLocation.GetRandomDirection();
    }

    private void calculateCameraPositions() {
        spawnBounds = new Vector3[]{
            new Vector3(position.x + spawnPadding, 0, -position.z - spawnPadding),          //topleftcorner
            new Vector3(position.x + size.x - spawnPadding, 0, -position.z - spawnPadding), //toprightcorner
            new Vector3(position.x + size.x - spawnPadding, 0, position.z + spawnPadding),  //bottomRightCorner
            new Vector3(position.x + spawnPadding, 0, position.z + spawnPadding)            //bottomLeftCorner
        };
        Ray[] rays = {
            Camera.main.ScreenPointToRay(new Vector3(0, 0, 0)),                         //topleft
            Camera.main.ScreenPointToRay(new Vector3(Screen.width, 0, 0)),              //topright
            Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0)), //botright
            Camera.main.ScreenPointToRay(new Vector3(0, Screen.height, 0))             //botleft
         };
        Ray topLeft = Camera.main.ScreenPointToRay(new Vector3(0, 0, 0));
        Ray topRight = Camera.main.ScreenPointToRay(new Vector3(Screen.width, 0, 0));
        Ray botRight = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0));
        Ray botLeft = Camera.main.ScreenPointToRay(new Vector3(0, Screen.height, 0));

        RaycastHit[] hits = new RaycastHit[4];

        for (int i = 0; i < rays.Length; ++i)
        {

            if (Physics.Raycast(rays[i].origin, rays[i].direction * 1000f, out hits[i]))
            {
                camBounds[i] = hits[i].point;
                curBounds[i] = camBounds[i];
                //Debug.DrawLine(new Vector3(0, 20, 0), pointTopRight, Color.green);
            }
        }
        if (camBounds.Length == 4)
        {
            if (camBounds[2].z > camBounds[3].z) camBounds[3].z = camBounds[2].z;
            else camBounds[2].z = camBounds[3].z;

            if (camBounds[0].z < camBounds[1].z) camBounds[1].z = camBounds[0].z;
            else camBounds[0].z = camBounds[1].z;
        }
    }
}
