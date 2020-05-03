using System.Collections.Generic;
using UnityEngine;
public class UnitPool : ObjectPool<UnitPool, UnitObject, Vector3>
{
    //static protected Dictionary<GameObject, UnitPool> s_PoolInstances = new Dictionary<GameObject, UnitPool>();

    //void Awake()
    //{
    //    //This allow to make Pool manually added in the scene still automatically findable & usable
    //    if (prefab != null && !s_PoolInstances.ContainsKey(prefab))
    //        s_PoolInstances.Add(prefab, this);
    //}

    //void OnDestroy()
    //{
    //    s_PoolInstances.Remove(prefab);
    //}

    ////initialPoolCount is only used when the objectpool don't exist
    //static public UnitPool GetObjectPool(GameObject prefab, int initialPoolCount = 10)
    //{
    //    UnitPool objPool = null;
    //    if (!s_PoolInstances.TryGetValue(prefab, out objPool))
    //    {
    //        GameObject obj = new GameObject(prefab.name + "_Pool");
    //        objPool = obj.AddComponent<UnitPool>();
    //        objPool.prefab = prefab;
    //        objPool.initialPoolCount = initialPoolCount;

    //        s_PoolInstances[prefab] = objPool;
    //    }

    //    return objPool;
    //}
}

public class UnitObject : PoolObject<UnitPool, UnitObject, Vector3>
{
    public Transform transform;
    public UnitScript unit;

    protected override void SetReferences()
    {
        transform = instance.transform;
        unit = instance.GetComponent<UnitScript>();
    }

    public override void WakeUp(Vector3 position)
    {
        transform.position = position;
        instance.SetActive(true);
        unit.Init();
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }
}
