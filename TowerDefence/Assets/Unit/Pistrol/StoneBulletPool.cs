using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class StoneBulletPool : ObjectPool<StoneBulletPool, StoneBulletObject, Vector3>
{
    public UnitScript unitScript;
    void Start()
    {
        transform.parent = null;

        for (int i = 0; i < initialPoolCount; i++)
        {
            StoneBulletObject newPoolObject = CreateNewPoolObject();
            pool.Add(newPoolObject);
        }
    }
}

public class StoneBulletObject : PoolObject<StoneBulletPool, StoneBulletObject, Vector3>
{
    public Transform transform;
    public StoneBulletScript script;
    protected override void SetReferences()
    {
        transform = instance.transform;
        script = instance.GetComponent<StoneBulletScript>();
        script.baseWeapon.onDespawned.Subscribe(_ => ReturnToPool());
    }

    public override void WakeUp(Vector3 pos)
    {
        script.Init(pos, objectPool.unitScript);
        instance.SetActive(true);
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }
}