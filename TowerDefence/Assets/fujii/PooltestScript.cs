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
    public List<SpriteObject> list = new List<SpriteObject>();
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            list.Add(Pop(Vector3.zero, sprite));
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            int i = UnityEngine.Random.Range(0, list.Count);
            Push(list[i]);
            list.RemoveAt(i);
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