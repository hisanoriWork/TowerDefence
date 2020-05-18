using UnityEngine;
using UniRx;
public class BoomerangPool : ObjectPool<BoomerangPool, BoomerangObject, Vector3>
{
    public UnitScript unitScript;
    public Transform throwPos;
    void Start()
    {
        transform.parent = null;
        for (int i = 0; i < initialPoolCount; i++)
        {
            BoomerangObject newPoolObject = CreateNewPoolObject();
            pool.Add(newPoolObject);
        }
    }
}

public class BoomerangObject : PoolObject<BoomerangPool, BoomerangObject, Vector3>
{
    public Transform transform;
    public BoomerangScript script;
    protected override void SetReferences()
    {
        transform = instance.transform;
        script = instance.GetComponent<BoomerangScript>();
        script.onDespawned.Subscribe(_ => ReturnToPool());
    }

    public override void WakeUp(Vector3 pos)
    {
        script.Init(pos, objectPool.unitScript, objectPool.throwPos);
        instance.SetActive(true);
    }

    public override void Sleep()
    {
        instance.SetActive(false);
    }
}
