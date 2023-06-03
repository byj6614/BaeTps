using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HomePoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic; //pool의 오브젝트를 담는 Dictionary 생성
    Dictionary<string, Transform> poolContainer;        //pool의 폴더
    Transform poolRoot;                                 //pool위 위치

    private void Awake()//초기화
    {
        poolDic=new Dictionary<string, ObjectPool<GameObject>>();
        poolContainer=new Dictionary<string, Transform>();
        poolRoot = new GameObject("PoolRoot").transform;
    }

    public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        //T형,포지션,회전,부모 를 받는다.
    {//T형으로 GameObject와 Component를 받는 Get생성 wherer로 Object형만 받도로 제한
        if (original is GameObject)
        {//original이 GameeObject일 때
            GameObject prefab = original as GameObject;//original을 T형에서 GameObject로 바꿔준다.
            string key = prefab.name;//현재 오브젝트에 이름을 key에 담아준다.

            if (!poolDic.ContainsKey(key))//poolDic에 key가 없다면 새로 생성해준다.
                CreatePool(key, prefab);
            GameObject obj = poolDic[key].Get();//poolDic에서 지정된 obj를 꺼내온다.
            obj.transform.parent = parent;//부모를 지정해주고
            obj.transform.position = position;//위치를 지정하고
            obj.transform.rotation = rotation;//회전을 지정해준다.
            return obj as T;//마무리로 obj를 T형으로 전환하고 내보내준다.
        }
        else if (original is Component)//original이 Component일 경우 
        {
            Component comp = original as Component;//original을 Component로 바꿔준다.
            string key = comp.gameObject.name;//받아온 Component의 이름을 key에 저장

            if (!poolDic.ContainsKey(key))//현재 poolDic에 key가 없을 시 새로 생성해준다.
                CreatePool(key, comp.gameObject);//현재 컴포넌트의 오브젝트를 넣어준다는것이 위와 다른점

            GameObject obj = poolDic[key].Get();//poolDic에서 지정된 obj를 꺼내온다
            obj.transform.parent = parent;//부모를 지정해주고
            obj.transform.position = position;//위치를 지정하고
            obj.transform.rotation = rotation;//회전을 지정해준다.
            return obj as T;//마무리로 obj를 T형으로 전환하고 내보내준다.
        }
        else
        {
            return null;//혹여나 Object에서 넣어야할게 아닌 다른것들이 들어갔을경우 null을 반환해준다.
        }
    }
    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object//부모를 지정받지 않고 함수를 실행할때
    {
        return Get<T>(original, position, rotation, null);
    }

    public T Get<T>(T original, Transform parent) where T : Object//위치와 회전을 받지않고 함수를 실행할때
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity, parent);
    }

    public T Get<T>(T original) where T : Object//T형(GameObject와 Component)만 받았을 때
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity, null);
    }

    public bool Release<T>(T instance) where T : Object     //이미 만들어져잇는 T형을 없애는 함수
    {
        if(instance is GameObject)//GameObject일경우
        {
            GameObject go = instance as GameObject;//instance를 GameObject로 바꿔준다.
            string key = go.name;  //key에 instance의 이름을 담아준다.
            
            if(!poolDic.ContainsKey(key))//poolDic안에 key를 담당하는 게 없을경우 flase를 내보낸다.
                return false;
            poolDic[key].Release(go);//poolDic안에 있을경우 Release로 지워준다.
            return true;//지우는걸 성공했으니 true를 반환한다.
        }
        else if(instance is Component)//instance를 Component로 바꿔준다.
        {
            Component component = instance as Component;//instance를 Component로 바꿔준다.
            string key = component.gameObject.name;//key에 Component를 가지고 있는 object의 이름을 담아준다.

            if (!poolDic.ContainsKey(key))//poolDic안에 key를 담당하는 게 없을경우 flase를 내보낸다.
                return false;

            poolDic[key].Release(component.gameObject);//poolDic안에 있을경우 Release로 지워준다.
            return true;//지우는걸 성공했으니 true를 반환한다.
        }
        else
        {
            return false;//GameObject와 Componet말고 다른 형태의 데이터를 받을경우 false를 반환
        }
    }

    
    private void CreatePool(string key, GameObject prefab)//새로운 pool을 만들어주는 함수
    {
        GameObject root = new GameObject(); //root를 초기화해준다.
        root.gameObject.name = $"{key}Container";//새로 만들어지는 pool의 이름
        root.transform.parent = poolRoot;//새로만들어지는 pool의 부모
        poolContainer.Add(key, root.transform);//poolContainer라는 폴더안에 생성

        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(//pool을 만들지 삭제할지 활성화할지 비활성화 하는 함수를 람다식으로 지정해준다.
            createFunc: ()=>//새로운 pool 생성
            {
                GameObject obj=Instantiate(prefab);//prefab을 새로이 생성
                obj.gameObject.name=key;//생성된 prefab이 obj에 들어감과 이름을 새로이 지정
                return obj;//생성완료
            },
            actionOnGet: (GameObject obj)=>//pool을 활성화
            {
                obj.gameObject.SetActive(true);//비활성화 된 상태인 obj를 활성화 해준다.
                obj.transform.parent = null;//부모에서 빠져나온다.
            },
            actionOnRelease: (GameObject obj) =>
            {
                obj.gameObject.SetActive(false);//활성화 된 상태인 obj를 다시 비활성화 해준다.
                obj.transform.parent = poolContainer[key];//부모에서 빠져나온 obj를 다시 부모로 넣어준다.
            },
            actionOnDestroy: (GameObject obj)=>
            {
                Destroy(obj);//오브젝트를 삭제시킨다.
            }
            );
        poolDic.Add(key, pool);//생성된 pool를 넣어준다.
    }
}
