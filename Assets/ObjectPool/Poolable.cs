using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Poolable : MonoBehaviour
{
    [SerializeField] float releaseTime;

    private ObjectPool pool;
    public ObjectPool Pool { get { return pool; } set { pool = value; } }

    private void OnEnable()
    {
        StartCoroutine(ReleaseTime());
    }
    IEnumerator ReleaseTime()
    {
       
        yield return new WaitForSeconds(releaseTime);
        pool.Release(this);
    }
}
