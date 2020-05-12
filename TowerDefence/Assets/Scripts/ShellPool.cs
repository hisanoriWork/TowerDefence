using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShellPool : ObjectPool<ShellPool,ShellObject,InfoToWeapon>
{
    
}

public class ShellObject :PoolObject<ShellPool,ShellObject,InfoToWeapon>
{

    public Transform transform;
    public ShellScript script;
    protected override void SetReferences()
    {
        transform = instance.transform;
        script = instance.GetComponent<ShellScript>();
    }

    public override void WakeUp(InfoToWeapon info)
    {
        instance.SetActive(true);
        script.Init(info.pos,info.layer,info.power);
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }
}
