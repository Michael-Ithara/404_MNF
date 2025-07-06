using System;
using UnityEngine;

public class Gem : MonoBehaviour, IItem
{
    public static event Action<int> onGemCollect;
    public int worth = 5;
    public void Collect()
    {
        onGemCollect.Invoke(worth);
        Destroy(gameObject);
    }
}
