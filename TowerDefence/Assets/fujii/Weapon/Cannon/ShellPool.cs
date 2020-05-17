using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
public class ShellPool : ObjectPool<ShellPool,ShellObject,Vector3>
{
    public UnitScript unitScript;
    void Start()
    {
        transform.parent = null;
    }
}

public class ShellObject :PoolObject<ShellPool,ShellObject,Vector3>
{
    public Transform transform;
    public ShellScript script;
    protected override void SetReferences()
    {
        transform = instance.transform;
        script = instance.GetComponent<ShellScript>();
        script.onDespawned.Subscribe(_ => ReturnToPool());
    }

    public override void WakeUp(Vector3 pos)
    {
        instance.SetActive(true);
        script.Init(pos,objectPool.unitScript);
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }
}
