using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    public Block blockPrefab;
    public int poolSize = 50;

    private Queue<Block> pool = new Queue<Block>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Block b = Instantiate(blockPrefab, transform);
            b.gameObject.SetActive(false);
            pool.Enqueue(b);
        }
    }

    public Block GetBlock()
    {
        if (pool.Count > 0)
        {
            Block b = pool.Dequeue();
            b.gameObject.SetActive(true);
            return b;
        }
        else
        {
            Block b = Instantiate(blockPrefab, transform);
            return b;
        }
    }

    public void ReturnBlock(Block b)
    {
        b.gameObject.SetActive(false);
        pool.Enqueue(b);
    }
}
