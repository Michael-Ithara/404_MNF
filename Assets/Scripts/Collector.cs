using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IItem item = collision.GetComponent<IItem>();
        
        if (item != null)
        {
            item.Collect();  // Collect the item

            // Play the shard sound effect
            SoundManager.instance.PlayShard(); 

            // Destroy the collected item (or the object this script is attached to)
            Destroy(collision.gameObject);  // Destroy the collected item
        }
    }
}
