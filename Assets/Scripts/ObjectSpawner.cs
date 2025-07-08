using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Linq;

public class ObjectSpawner : MonoBehaviour
{
    public enum ObjectType { SmallGem, BigGem, Enemy }

    public Tilemap tilemap;
    public GameObject[] objectPrefabs; // 0=SmallGem, 1=BigGem, 2=Enemy
    public float bigGemProbability = 0.2f; // 20% chance of spawning big gem
    public float enemyProbability = 0.1f;
    public int maxObjects = 5;
    public float gemLifeTime = 10f; // Only for gems
    public float spawnInterval = 0.5f;
    public float padding = 0.5f; // Padding to move spawn positions away from edges
    public float zPosition = -0.1f; // Negative Z for below the platform

    private List<Vector3> validSpawnPositions = new List<Vector3>();
    private List<GameObject> spawnObjects = new List<GameObject>();
    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        GatherValidPositions();
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

    private bool PositionHasObject(Vector3 positionToCheck)
    {
        return spawnObjects.Any(checkObj => checkObj && Vector3.Distance(checkObj.transform.position, positionToCheck) < 1.0f);
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
        if (validSpawnPositions.Count == 0) return;

        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        while (!validPositionFound && validSpawnPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPositions.Count);
            Vector3 potentialPosition = validSpawnPositions[randomIndex];
            Vector3 leftPosition = potentialPosition + Vector3.left;
            Vector3 rightPosition = potentialPosition + Vector3.right;

            if (!PositionHasObject(leftPosition) && !PositionHasObject(rightPosition))
            {
                spawnPosition = potentialPosition + new Vector3(padding, 0, 0); // Apply padding to X position
                spawnPosition.z = zPosition; // Set Z to a negative value (below the platform)
                validPositionFound = true;
                validSpawnPositions.RemoveAt(randomIndex);
            }
        }

        if (validPositionFound)
        {
            ObjectType objectType = RandomObjectType();
            GameObject gameObject = Instantiate(objectPrefabs[(int)objectType], spawnPosition, Quaternion.identity);

            // Ensure the Z position is correctly set, even if the prefab has an offset
            Vector3 correctedPosition = gameObject.transform.position;
            correctedPosition.z = zPosition; // Ensure Z remains negative
            gameObject.transform.position = correctedPosition;

            // Set the sorting layer and order in layer for visibility
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Default"; // Set appropriate layer
                spriteRenderer.sortingOrder = 5; // Ensure it's above background
            }

            spawnObjects.Add(gameObject);

            // Destroy gems only after a certain time
            if (objectType != ObjectType.Enemy)
            {
                StartCoroutine(DestroyObjectAfterTime(gameObject, gemLifeTime));
            }
        }
    }

    private IEnumerator DestroyObjectAfterTime(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);

        if (gameObject)
        {
            spawnObjects.Remove(gameObject);
            Vector3 resetPosition = gameObject.transform.position;
            resetPosition.z = 0; // Reset Z to 0 before removing the object
            validSpawnPositions.Add(resetPosition);
            Destroy(gameObject);
        }
    }

    private void GatherValidPositions()
    {
        validSpawnPositions.Clear();
        BoundsInt boundsInt = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(boundsInt);
        Vector3 start = tilemap.CellToWorld(new Vector3Int(boundsInt.xMin, boundsInt.yMin, 0));

        for (int x = 0; x < boundsInt.size.x; x++)
        {
            for (int y = 0; y < boundsInt.size.y; y++)
            {
                TileBase tile = allTiles[x + y * boundsInt.size.x];
                if (tile != null)
                {
                    // Apply padding to adjust spawn positions slightly inside the tile
                    Vector3 place = start + new Vector3(x, y, 0) + new Vector3(padding, 0, 0);
                    place.z = 0; // Keep Z = 0 for tilemap position
                    validSpawnPositions.Add(place);

                    // Optional: log the valid spawn position for debugging
                    Debug.Log("Valid spawn position: " + place);
                }
            }
        }
    }
}
