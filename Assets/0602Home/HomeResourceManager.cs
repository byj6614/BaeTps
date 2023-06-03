using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeResourceManager : MonoBehaviour
{
    Dictionary<string, Object> resources = new Dictionary<string, Object>();//resources를 담는 Dictionary를 생성

    public T Load<T>(string path) where T : Object//T형을 불러오는 함수.
    {
        string key = $"{typeof(T)}.{path}";//key의 이름을 넣어준다. 

        if (resources.ContainsKey(key))//Dictionary에 key가 담당하는 Object가 없을경우
            return resources[key] as T;//새로이 만들어준다.

        T resource = Resources.Load<T>(path);//리소스를 불러와서 resource에 담아준다.
        resources.Add(key, resource);//resources에 새로이 불러운 resource를 담아준다.
        return resource;//resource를 내보낸다.
    }
    public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent, bool pooling = false) where T : Object
    {//새로이 추가 하는 함수 (T형 origina, 위치, 회전 , 부모 ,pool=false(비활성화를 미리 선언)) Object로 제한해주며
        if (pooling)//pooling이 true일 경우 
            return GameManager.Pool.Get(original, position, rotation, parent);//게임 매니저에 있는 Pool을 가져온다.
        else//아닌경우
            return Object.Instantiate(original, position, rotation, parent);//새로이 생성해준다.
    }
    //부모를 지정안하는 경우
    public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, position, rotation, null, pooling);
    }
    //회전과 위치를 지정 안하는 경우
    public new T Instantiate<T>(T original, Transform parent, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, Vector3.zero, Quaternion.identity, parent, pooling);
    }
    //T형만 지정하는 경우
    public T Instantiate<T>(T original, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, Vector3.zero, Quaternion.identity, null, pooling);
    }

    public void Destroy(GameObject go)//Resource안에 Object를 제거하는 함수
    {
        if (GameManager.Pool.IsContain(go))//poolContains안에 있는 경우
            GameManager.Pool.Release(go);//Release를 통해 지워준다.
        else
            GameObject.Destroy(go);//위 조건이 아닐경우 제거해버린다.
    }

    public void Destroy(GameObject go, float delay)//약간의 시간을 주고 제거하는 함수
    {                                               
        if (GameManager.Pool.IsContain(go))
            StartCoroutine(DelayReleaseRoutine(go, delay));//위와 다르게 코루틴을 통하여 딜레이르 주어 제거
        else
            GameObject.Destroy(go, delay);
    }

    IEnumerator DelayReleaseRoutine(GameObject go, float delay)//코루틴을 사용해서 딜레이 후 제거
    {
        yield return new WaitForSeconds(delay);//delay만큼 기다린다.
        GameManager.Pool.Release(go);//그후 제거한다.
    }

    public void Destroy(Component component, float delay = 0f)//기본적으로 딜레이를 0으로 주고 제거하는 함수를 미리 제작해준다.
    {
        Component.Destroy(component, delay);
    }
}
