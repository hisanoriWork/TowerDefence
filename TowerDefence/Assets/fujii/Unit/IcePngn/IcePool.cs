using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
public class IcePool : ObjectPool<IcePool, IceObject, Vector3>
{
    public UnitScript unitScript;
    void Start()
    {
        transform.parent = null;

        for (int i = 0; i < initialPoolCount; i++)
        {
            IceObject newPoolObject = CreateNewPoolObject();
            pool.Add(newPoolObject);
        }
    }
}

public class IceObject : PoolObject<IcePool, IceObject, Vector3>
{
    public Transform transform;
    public IceScript script;
    protected override void SetReferences()
    {
        transform = instance.transform;
        script = instance.GetComponent<IceScript>();
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
