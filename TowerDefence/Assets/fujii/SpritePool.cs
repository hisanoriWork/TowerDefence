using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteInfo
{
    public Vector3 position;
    public Sprite sprite;

    public SpriteInfo(Vector3 position,Sprite sprite)
    {
        this.position = position;
        this.sprite = sprite;
    }
}

public class SpritePool : ObjectPool<SpritePool, SpriteObject, SpriteInfo>
{}

public class SpriteObject : PoolObject<SpritePool, SpriteObject, SpriteInfo>
{
    public Transform transform;
    public SpriteRenderer spriteRenderer;
    protected override void SetReferences()
    {
        transform = instance.transform;
        spriteRenderer = instance.GetComponent<SpriteRenderer>();
    }

    public override void WakeUp(SpriteInfo spriteInfo)
    {
        transform.position = spriteInfo.position;
        spriteRenderer.sprite = spriteInfo.sprite;
        instance.SetActive(true);
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }
}
