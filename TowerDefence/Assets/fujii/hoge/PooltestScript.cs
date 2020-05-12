using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PooltestScript : MonoBehaviour
{
    public MasterDataScript masterData;
    public SpritePool spritePool;
    public Sprite sprite;

    void Start()
    {
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Vector3 pos = Vector3.zero;
            for(int i = 0;i<100;i++)
            {
                if (spritePool.pool[i].inPool)
                {
                    pos.x = i * 0.8f;
                    spritePool.pool[i] = Pop(pos, sprite);
                    break;
                }
            }
            
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            int num = 0;
            foreach(var obj in spritePool.pool)
            {
                if (!obj.inPool) num++;
            }
            
            int i;
            while (true)
            {
                i = UnityEngine.Random.Range(0, spritePool.pool.Count);
                if (!spritePool.pool[i].inPool) break;
            }
            Push(spritePool.pool[i]);
            spritePool.pool.RemoveAt(i);
            
        }
    }
    public SpriteObject Pop(Vector3 position,Sprite sprite)
    {
        return spritePool.Pop(new SpriteInfo(position, sprite));
    }

    public void Push(SpriteObject spriteObject)
    {
        spriteObject.ReturnToPool();
    }
}