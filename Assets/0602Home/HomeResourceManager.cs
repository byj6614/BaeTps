using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeResourceManager : MonoBehaviour
{
    Dictionary<string, Object> resources = new Dictionary<string, Object>();//resources�� ��� Dictionary�� ����

    public T Load<T>(string path) where T : Object//T���� �ҷ����� �Լ�.
    {
        string key = $"{typeof(T)}.{path}";//key�� �̸��� �־��ش�. 

        if (resources.ContainsKey(key))//Dictionary�� key�� ����ϴ� Object�� �������
            return resources[key] as T;//������ ������ش�.

        T resource = Resources.Load<T>(path);//���ҽ��� �ҷ��ͼ� resource�� ����ش�.
        resources.Add(key, resource);//resources�� ������ �ҷ��� resource�� ����ش�.
        return resource;//resource�� ��������.
    }
    public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent, bool pooling = false) where T : Object
    {//������ �߰� �ϴ� �Լ� (T�� origina, ��ġ, ȸ�� , �θ� ,pool=false(��Ȱ��ȭ�� �̸� ����)) Object�� �������ָ�
        if (pooling)//pooling�� true�� ��� 
            return GameManager.Pool.Get(original, position, rotation, parent);//���� �Ŵ����� �ִ� Pool�� �����´�.
        else//�ƴѰ��
            return Object.Instantiate(original, position, rotation, parent);//������ �������ش�.
    }
    //�θ� �������ϴ� ���
    public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, position, rotation, null, pooling);
    }
    //ȸ���� ��ġ�� ���� ���ϴ� ���
    public new T Instantiate<T>(T original, Transform parent, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, Vector3.zero, Quaternion.identity, parent, pooling);
    }
    //T���� �����ϴ� ���
    public T Instantiate<T>(T original, bool pooling = false) where T : Object
    {
        return Instantiate<T>(original, Vector3.zero, Quaternion.identity, null, pooling);
    }

    public void Destroy(GameObject go)//Resource�ȿ� Object�� �����ϴ� �Լ�
    {
        if (GameManager.Pool.IsContain(go))//poolContains�ȿ� �ִ� ���
            GameManager.Pool.Release(go);//Release�� ���� �����ش�.
        else
            GameObject.Destroy(go);//�� ������ �ƴҰ�� �����ع�����.
    }

    public void Destroy(GameObject go, float delay)//�ణ�� �ð��� �ְ� �����ϴ� �Լ�
    {                                               
        if (GameManager.Pool.IsContain(go))
            StartCoroutine(DelayReleaseRoutine(go, delay));//���� �ٸ��� �ڷ�ƾ�� ���Ͽ� �����̸� �־� ����
        else
            GameObject.Destroy(go, delay);
    }

    IEnumerator DelayReleaseRoutine(GameObject go, float delay)//�ڷ�ƾ�� ����ؼ� ������ �� ����
    {
        yield return new WaitForSeconds(delay);//delay��ŭ ��ٸ���.
        GameManager.Pool.Release(go);//���� �����Ѵ�.
    }

    public void Destroy(Component component, float delay = 0f)//�⺻������ �����̸� 0���� �ְ� �����ϴ� �Լ��� �̸� �������ش�.
    {
        Component.Destroy(component, delay);
    }
}
