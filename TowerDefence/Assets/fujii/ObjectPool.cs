using System;
using System.Collections.Generic;
using UnityEngine;

/*概要
 * ObjectPool<ObjectPool型，PoolObject型，Pop時に利用する情報>
 * ObjectPool<ObjectPool型，PoolObject型>
 * ObjectPooltはオブジェクトのインスタンスをプールに保存しておくことで，
 * 利用したいときにプールから取り出していつでも利用できるようにする機能があります．
 * また利用を解除するときはプールに戻し，いつでも再利用することができるので，
 * プレハブを生成するという重い処理の回数をできるだけ少なくすることができます．
 * 以下軽くObjectPoolのフィールド，メソッドの説明をします．
 * ・prefab：プールに入れるプレハブ
 * ・initialPoolCount：最初にプールに入れられるインスタンスの数
 * ・Pop(TInfo),Pop()：プール内のオブジェクトのインスタンスをWakeUpし取り出します
 * ・Push()：インスタンスの利用を解除するときはSleepをしてプール内に戻します
 * ・CreateNewPoolObject()：プレハブのインスタンスを生成し，プールに入れる
 * （親はObjectPoolがある場所で,SetReferences(Pool)してSleep()する．）
 * 
 * PoolObject<ObjectPool型，PoolObject型，Pop時に利用する情報>
 * PoolObject<ObjectPool型，PoolObject型>
 * PoolObjectとは，プレハブをObjectPoolが利用しやすいように形を変えたもので，
 * ObjectPoolのインスタンスがプールに入れられます．
 * 以下軽くPoolObjectのメソッドの説明をします．
 * ・WakeUp(TInfo)，WakeUp()：Pop時に呼ばれます
 * ・Sleep()：Push時に呼ばれます
 * ・SetReferences(Pool)：thisのプールをセットし，GetComponentなどの参照をPoolObjectに保存して置いたりします
 * ・SetReferences()：SetReferences(Pool)時に呼ばれます
 * ・ReturnToPool()：プールにPushします
 */
public abstract class ObjectPool<TPool, TObject, TInfo> : ObjectPool<TPool, TObject>
    where TPool : ObjectPool<TPool, TObject, TInfo>
    where TObject : PoolObject<TPool, TObject, TInfo>, new()
{
    void Start()
    {
        for (int i = 0; i < initialPoolCount; i++)
        {
            TObject newPoolObject = CreateNewPoolObject();
            pool.Add(newPoolObject);
        }
    }

    public virtual TObject Pop(TInfo info)
    {
        for (int i = 0; i < pool.Count; i++)if (pool[i].inPool)
        {    
            pool[i].inPool = false;
            pool[i].WakeUp(info);
            return pool[i]; 
        }

        TObject newPoolObject = CreateNewPoolObject();
        pool.Add(newPoolObject);
        newPoolObject.inPool = false;
        newPoolObject.WakeUp(info);
        return newPoolObject;
    }
}

public abstract class ObjectPool<TPool, TObject> : MonoBehaviour
    where TPool : ObjectPool<TPool, TObject>
    where TObject : PoolObject<TPool, TObject>, new()
{
    public GameObject prefab; //対応したプレハブ
    public int initialPoolCount = 10; //初期に作られるプールの数
    [HideInInspector]
    public List<TObject> pool = new List<TObject>(); //オブジェクトが入るプール

    void Start()
    {
        for (int i = 0; i < initialPoolCount; i++)
        {
            TObject newPoolObject = CreateNewPoolObject();
            pool.Add(newPoolObject);
        }
    }

    public virtual TObject Pop()
    {
        for (int i = 0; i < pool.Count; i++)if (pool[i].inPool)
        {
            pool[i].inPool = false;
            pool[i].WakeUp();
            return pool[i];
        }

        TObject newPoolObject = CreateNewPoolObject();
        pool.Add(newPoolObject);
        newPoolObject.inPool = false;
        newPoolObject.WakeUp();
        return newPoolObject;
    }

    public virtual void Push(TObject poolObject)
    {
        poolObject.inPool = true;
        poolObject.Sleep();
    }

    protected TObject CreateNewPoolObject()
    {
        TObject newPoolObject = new TObject();
        newPoolObject.instance = Instantiate(prefab);
        newPoolObject.instance.transform.SetParent(transform);
        newPoolObject.inPool = true;
        newPoolObject.SetReferences(this as TPool);
        newPoolObject.Sleep();
        return newPoolObject;
    }
}

[Serializable]
public abstract class PoolObject<TPool, TObject, TInfo> : PoolObject<TPool, TObject>
    where TPool : ObjectPool<TPool, TObject, TInfo>
    where TObject : PoolObject<TPool, TObject, TInfo>, new()
{
    public virtual void WakeUp(TInfo info)
    { }
}

[Serializable]
public abstract class PoolObject<TPool, TObject>
    where TPool : ObjectPool<TPool, TObject>
    where TObject : PoolObject<TPool, TObject>, new()
{
    public bool inPool;
    public GameObject instance;
    public TPool objectPool;

    public void SetReferences(TPool pool)
    {
        objectPool = pool;
        SetReferences();
    }

    protected virtual void SetReferences()
    { }

    public virtual void WakeUp()
    { }

    public virtual void Sleep()
    { }

    public virtual void ReturnToPool()
    {
        TObject thisObject = this as TObject;
        objectPool.Push(thisObject);
    }
}
