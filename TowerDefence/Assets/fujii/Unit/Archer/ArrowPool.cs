using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
public class ArrowPool : ObjectPool<ArrowPool, ArrowObject, Vector3>
{
    public UnitScript unitScript;
    void Start()
    {
        transform.parent = null;

        for (int i = 0; i < initialPoolCount; i++)
        {
            ArrowObject newPoolObject = CreateNewPoolObject();
            pool.Add(newPoolObject);
        }
    }
}

public class ArrowObject : PoolObject<ArrowPool, ArrowObject, Vector3>
{
    public Transform transform;
    public ArrowScript script;
    protected override void SetReferences()
    {
        transform = instance.transform;
        script = instance.GetComponent<ArrowScript>();
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
