using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuickPathBuilder : MonoBehaviour
{
    [Header("Core Prefabs")]
    public GameObject playerPrefab;    // Reference to the player prefab
    public GameObject enemyPrefab;     // Reference to the enemy prefab

    [Header("Path Definition")]
    // Path now only contains a list of BlockEntry to spawn player and enemy
    public List<BlockEntry> path = new List<BlockEntry>
    {
        new BlockEntry { type = "Player", offset = Vector2.zero },
        new BlockEntry { type = "Enemy", offset = new Vector2(2f, 0f) }  // Enemy offset from player
    };

    [Header("Layout Settings")]
    public Vector2 startPos = new Vector2(0, -3);
    public float spacingX = 4f;
    public float spawnPadding = 0.05f;

    void Start()
    {
        // Get the camera's width to help with layout
        float camLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float camRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float camWidth = camRight - camLeft;

        int blockCount = path.Count;
        float screenPadding = 1f;

        Vector2 cursor = new Vector2(camLeft + screenPadding / 2f, startPos.y);

        // Generate the path using player and enemy prefabs
        for (int i = 0; i < blockCount; i++)
        {
            BlockEntry entry = path[i];

            // Set the spawn position for the current entry (player or enemy)
            Vector3 spawnPos = new Vector3(cursor.x + entry.offset.x, cursor.y + entry.offset.y, 0f);
            Debug.Log($"Spawning {entry.type} at position: {spawnPos}");  // Debug log for spawn position

            // Spawn the player or enemy based on the entry type
            GameObject blk = null;
            if (entry.type == "Player" && playerPrefab != null)
            {
                blk = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
                blk.name = "Player";
                Debug.Log("Player spawned");
            }
            else if (entry.type == "Enemy" && enemyPrefab != null)
            {
                blk = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                blk.name = "Enemy";
                Debug.Log("Enemy spawned");
            }

            // Handle spawning issues (if prefab is missing)
            if (blk == null)
            {
                Debug.LogError($"Missing prefab for type \"{entry.type}\"!");
                continue;
            }

            cursor.x += spacingX;  // Move to the next horizontal position
        }

        Debug.Log($"Path generated with {path.Count} entries.");
    }
}

public class BlockEntry
{
    public string type;     // Type of the entry ("Player" or "Enemy")
    public Vector2 offset;  // Offset for positioning
}

