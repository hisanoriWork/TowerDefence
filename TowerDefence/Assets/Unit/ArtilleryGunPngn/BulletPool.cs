using UniRx;
using UnityEngine;
public class BulletPool : ObjectPool<BulletPool, BulletObject, Vector3>
{
    public UnitScript unitScript;
    void Start()
    {
        transform.parent = null;

        for (int i = 0; i < initialPoolCount; i++)
        {
            BulletObject newPoolObject = CreateNewPoolObject();
            pool.Add(newPoolObject);
        }
    }
}

public class BulletObject : PoolObject<BulletPool, BulletObject, Vector3>
{
    public Transform transform;
    public BulletScript script;
    protected override void SetReferences()
    {
        transform = instance.transform;
        script = instance.GetComponent<BulletScript>();
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
