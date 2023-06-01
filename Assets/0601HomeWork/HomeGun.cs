using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeGun : MonoBehaviour
{
    [SerializeField] float maxDistance; //���� �����Ÿ�
    [SerializeField] int damage;

    public void Fire()
    {
        RaycastHit hit;     //�����ɽ�Ʈ ����
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit,maxDistance))
        {   //����ī�޶�����ġ���� , ����ī�޶��� ������ hit�� �������� maxDistance����
            IHitable hittable = hit.transform.GetComponent<IHitable>();

            hittable?.Hit(hit, damage);//IHitable�� ���� �¾������ hit�� Ʈ���� ��� ����
        }
    }
    
}
