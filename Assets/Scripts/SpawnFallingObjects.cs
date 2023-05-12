using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFallingObjects : MonoBehaviour {

    public static SpawnFallingObjects Instance;

    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float maxSpawnInterval = 1.5f;

    private float _screenWidth;
    private float _nextSpawnTime;

    private bool startSwarm = false;

    // List of all comets
    public List<GameObject> cometList = new List<GameObject>();

    private void Awake() {
        Instance = this;

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state) {
        startSwarm = state == GameState.GameLoop;
    }

    private void Start() {
        // Makes sure that the triangle doesn't spawn on the edges of the screen
        _screenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - (objectPrefab.GetComponent<SpriteRenderer>().bounds.extents.x)*2, 0f, 0f)).x;
        _nextSpawnTime = 0f;
    }

    private void Update() {
        // If the game state is not GameLoop then do nothing
        if (!startSwarm) return;

        if (Time.time >= _nextSpawnTime) {
            // Play SFX On Spawn

            SpawnObject();
            Debug.Log("Next spawn time: "+_nextSpawnTime);
            _nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void SpawnObject() {
        float xPos = Random.Range(-_screenWidth, _screenWidth);
        Vector3 spawnPosition = new Vector3(xPos, 6f, 0f);

        GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // Add to list
        cometList.Add(newObject);
    }
}
