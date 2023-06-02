using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
public class ObjectPool : MonoBehaviour
{
    [SerializeField] Poolable poolAblePrefab;

    [SerializeField] int pollSize;
    [SerializeField] int maxSize;

    private Stack<Poolable> objectPool=new Stack<Poolable>();

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for(int i=0;i<pollSize; i++)
        {
            Poolable poolable = Instantiate(poolAblePrefab);
            poolable.gameObject.SetActive(false);
            poolable.transform.SetParent(transform);
            poolable.Pool = this;
            objectPool.Push(poolable);
        }
    }

    public Poolable Get()
    {
        if(objectPool.Count > 0)
        {
            Poolable poolable = objectPool.Pop();
            poolable.gameObject.SetActive(true);
            poolable.transform.parent = null;
            return poolable;
        }
        else
        {
            Poolable poolable =Instantiate(poolAblePrefab);
            poolable.Pool = this;
            return poolable;
        }
    }

    public void Release(Poolable poolable)
    {
        if(objectPool.Count<maxSize)
        {
            poolable.gameObject.SetActive(false);
            poolable.transform.SetParent(transform);
            objectPool.Push(poolable);
        }
        else
        {
            Destroy(poolable.gameObject);
        }
    }
}
