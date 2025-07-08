using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Spawner : MonoBehaviour
{
    public enum ObjectType { SmallGem, BigGem, Enemy }

    public GameObject movingPlatform;  // Reference to the MovingPlatform object
    public GameObject[] objectPrefabs; // 0=SmallGem, 1=BigGem, 2=Enemy
    public float bigGemProbability = 0.2f; // 20% chance of spawning big gem
    public float enemyProbability = 0.1f;
    public int maxObjects = 5;
    public float gemLifeTime = 10f; // Only for gems
    public float spawnInterval = 0.5f;
    public float padding = 0.5f; // Padding to move spawn positions away from edges
    public float zPosition = -0.1f; // Negative Z for objects to appear below the platform

    private List<GameObject> spawnObjects = new List<GameObject>();
    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObjectsIfNeeded());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning && ActiveObjectCount() < maxObjects)
        {
            StartCoroutine(SpawnObjectsIfNeeded());
        }
    }

    private int ActiveObjectCount()
    {
        spawnObjects.RemoveAll(item => item == null);
        return spawnObjects.Count;
    }

    private IEnumerator SpawnObjectsIfNeeded()
    {
        isSpawning = true;
        while (ActiveObjectCount() < maxObjects)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
        isSpawning = false;
    }

    private ObjectType RandomObjectType()
    {
        float randomChoice = Random.value;

        if (randomChoice <= enemyProbability)
        {
            return ObjectType.Enemy;
        }
        else if (randomChoice <= (enemyProbability + bigGemProbability))
        {
            return ObjectType.BigGem;
        }
        else
        {
            return ObjectType.SmallGem;
        }
    }

    private void SpawnObject()
    {
        if (movingPlatform == null) return;

        // Spawn at platform's current position
        Vector3 spawnPosition = movingPlatform.transform.position;

        // Apply padding to the spawn position and ensure it is visible
        spawnPosition.x += padding;
        spawnPosition.z = zPosition; // Ensure Z remains negative to appear below the platform

        // Ensure the object is instantiated with a corrected Z position
        GameObject gameObject = Instantiate(objectPrefabs[(int)RandomObjectType()], spawnPosition, Quaternion.identity);

        // Correct the Z position after instantiation
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, zPosition);

        // Set the sorting layer and order in layer for visibility
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Default"; // Set appropriate layer
            spriteRenderer.sortingOrder = 5; // Ensure it's above background
        }

        spawnObjects.Add(gameObject);

        // Destroy gems only after a certain time
        if (gameObject.GetComponent<ObjectType>() != ObjectType.Enemy)
        {
            StartCoroutine(DestroyObjectAfterTime(gameObject, gemLifeTime));
        }
    }

    private IEnumerator DestroyObjectAfterTime(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);

        if (gameObject)
        {
            spawnObjects.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
