using UnityEngine;
using UniRx;
public class BombPool : ObjectPool<BombPool, BombObject, Vector3>
{
    public UnitScript unitScript;
    void Start()
    {
        transform.parent = null;

        for (int i = 0; i < initialPoolCount; i++)
        {
            BombObject newPoolObject = CreateNewPoolObject();
            pool.Add(newPoolObject);
        }
    }
}

public class BombObject : PoolObject<BombPool, BombObject, Vector3>
{
    public Transform transform;
    public BombScript script;
    protected override void SetReferences()
    {
        transform = instance.transform;
        script = instance.GetComponent<BombScript>();
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
