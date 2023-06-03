using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HomePoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic; //pool�� ������Ʈ�� ��� Dictionary ����
    Dictionary<string, Transform> poolContainer;        //pool�� ����
    Transform poolRoot;                                 //pool�� ��ġ

    private void Awake()//�ʱ�ȭ
    {
        poolDic=new Dictionary<string, ObjectPool<GameObject>>();
        poolContainer=new Dictionary<string, Transform>();
        poolRoot = new GameObject("PoolRoot").transform;
    }

    public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        //T��,������,ȸ��,�θ� �� �޴´�.
    {//T������ GameObject�� Component�� �޴� Get���� wherer�� Object���� �޵��� ����
        if (original is GameObject)
        {//original�� GameeObject�� ��
            GameObject prefab = original as GameObject;//original�� T������ GameObject�� �ٲ��ش�.
            string key = prefab.name;//���� ������Ʈ�� �̸��� key�� ����ش�.

            if (!poolDic.ContainsKey(key))//poolDic�� key�� ���ٸ� ���� �������ش�.
                CreatePool(key, prefab);
            GameObject obj = poolDic[key].Get();//poolDic���� ������ obj�� �����´�.
            obj.transform.parent = parent;//�θ� �������ְ�
            obj.transform.position = position;//��ġ�� �����ϰ�
            obj.transform.rotation = rotation;//ȸ���� �������ش�.
            return obj as T;//�������� obj�� T������ ��ȯ�ϰ� �������ش�.
        }
        else if (original is Component)//original�� Component�� ��� 
        {
            Component comp = original as Component;//original�� Component�� �ٲ��ش�.
            string key = comp.gameObject.name;//�޾ƿ� Component�� �̸��� key�� ����

            if (!poolDic.ContainsKey(key))//���� poolDic�� key�� ���� �� ���� �������ش�.
                CreatePool(key, comp.gameObject);//���� ������Ʈ�� ������Ʈ�� �־��شٴ°��� ���� �ٸ���

            GameObject obj = poolDic[key].Get();//poolDic���� ������ obj�� �����´�
            obj.transform.parent = parent;//�θ� �������ְ�
            obj.transform.position = position;//��ġ�� �����ϰ�
            obj.transform.rotation = rotation;//ȸ���� �������ش�.
            return obj as T;//�������� obj�� T������ ��ȯ�ϰ� �������ش�.
        }
        else
        {
            return null;//Ȥ���� Object���� �־���Ұ� �ƴ� �ٸ��͵��� ������� null�� ��ȯ���ش�.
        }
    }
    public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object//�θ� �������� �ʰ� �Լ��� �����Ҷ�
    {
        return Get<T>(original, position, rotation, null);
    }

    public T Get<T>(T original, Transform parent) where T : Object//��ġ�� ȸ���� �����ʰ� �Լ��� �����Ҷ�
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity, parent);
    }

    public T Get<T>(T original) where T : Object//T��(GameObject�� Component)�� �޾��� ��
    {
        return Get<T>(original, Vector3.zero, Quaternion.identity, null);
    }

    public bool Release<T>(T instance) where T : Object     //�̹� ��������մ� T���� ���ִ� �Լ�
    {
        if(instance is GameObject)//GameObject�ϰ��
        {
            GameObject go = instance as GameObject;//instance�� GameObject�� �ٲ��ش�.
            string key = go.name;  //key�� instance�� �̸��� ����ش�.
            
            if(!poolDic.ContainsKey(key))//poolDic�ȿ� key�� ����ϴ� �� ������� flase�� ��������.
                return false;
            poolDic[key].Release(go);//poolDic�ȿ� ������� Release�� �����ش�.
            return true;//����°� ���������� true�� ��ȯ�Ѵ�.
        }
        else if(instance is Component)//instance�� Component�� �ٲ��ش�.
        {
            Component component = instance as Component;//instance�� Component�� �ٲ��ش�.
            string key = component.gameObject.name;//key�� Component�� ������ �ִ� object�� �̸��� ����ش�.

            if (!poolDic.ContainsKey(key))//poolDic�ȿ� key�� ����ϴ� �� ������� flase�� ��������.
                return false;

            poolDic[key].Release(component.gameObject);//poolDic�ȿ� ������� Release�� �����ش�.
            return true;//����°� ���������� true�� ��ȯ�Ѵ�.
        }
        else
        {
            return false;//GameObject�� Componet���� �ٸ� ������ �����͸� ������� false�� ��ȯ
        }
    }

    
    private void CreatePool(string key, GameObject prefab)//���ο� pool�� ������ִ� �Լ�
    {
        GameObject root = new GameObject(); //root�� �ʱ�ȭ���ش�.
        root.gameObject.name = $"{key}Container";//���� ��������� pool�� �̸�
        root.transform.parent = poolRoot;//���θ�������� pool�� �θ�
        poolContainer.Add(key, root.transform);//poolContainer��� �����ȿ� ����

        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(//pool�� ������ �������� Ȱ��ȭ���� ��Ȱ��ȭ �ϴ� �Լ��� ���ٽ����� �������ش�.
            createFunc: ()=>//���ο� pool ����
            {
                GameObject obj=Instantiate(prefab);//prefab�� ������ ����
                obj.gameObject.name=key;//������ prefab�� obj�� ���� �̸��� ������ ����
                return obj;//�����Ϸ�
            },
            actionOnGet: (GameObject obj)=>//pool�� Ȱ��ȭ
            {
                obj.gameObject.SetActive(true);//��Ȱ��ȭ �� ������ obj�� Ȱ��ȭ ���ش�.
                obj.transform.parent = null;//�θ𿡼� �������´�.
            },
            actionOnRelease: (GameObject obj) =>
            {
                obj.gameObject.SetActive(false);//Ȱ��ȭ �� ������ obj�� �ٽ� ��Ȱ��ȭ ���ش�.
                obj.transform.parent = poolContainer[key];//�θ𿡼� �������� obj�� �ٽ� �θ�� �־��ش�.
            },
            actionOnDestroy: (GameObject obj)=>
            {
                Destroy(obj);//������Ʈ�� ������Ų��.
            }
            );
        poolDic.Add(key, pool);//������ pool�� �־��ش�.
    }
}
