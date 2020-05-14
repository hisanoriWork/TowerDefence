using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ShellPool : ObjectPool<ShellPool,ShellObject,InfoToWeapon>
{
    
}

public class ShellObject :PoolObject<ShellPool,ShellObject,InfoToWeapon>
{

    public Transform transform;
    public ShellScript script;
    public UnityEvent InPool;
    protected override void SetReferences()
    {
        transform = instance.transform;
        script = instance.GetComponent<ShellScript>();
        InPool = new UnityEvent();
        InPool.AddListener(ReturnToPool);
    }

    public override void WakeUp(InfoToWeapon info)
    {
        instance.SetActive(true);
        script.Init(info.pos, info.layer, info.power,InPool);
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }
}
