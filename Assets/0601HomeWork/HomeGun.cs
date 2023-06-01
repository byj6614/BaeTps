using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeGun : MonoBehaviour
{
    //�����ɽ�Ʈ�� ���� hit ����
    [SerializeField] float maxDistance; //���� �����Ÿ�
    [SerializeField] int damage;        //���� ������
    //��ƼŬ�� ���� �ѱ��� �ǰݽ� ����Ʈ ����
    [SerializeField] ParticleSystem hitEffect;      //���� �¾��� �� ����Ʈ ��ƼŬ
    [SerializeField] ParticleSystem muzzleEffect;   //���� �߻�Ǿ��� �� ���񿡼� ������ ����Ʈ


    public void Fire()
    {
        muzzleEffect.Play();//���񿡼��� �߻�ʰ� ���ÿ� ����Ʈ�� ���;� �ϹǷ� Fire �ߵ��� �ٷ� ����
        RaycastHit hit;     //�����ɽ�Ʈ ����
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit,maxDistance))
        {   //����ī�޶�����ġ���� , ����ī�޶��� ������ hit�� �������� maxDistance����
            IHitable hittable = hit.transform.GetComponent<IHitable>();
            ParticleSystem effect=Instantiate(hitEffect,hit.point,Quaternion.LookRotation(hit.normal));
            //��ƼŬ �ý��� ����Ʈ ������Ʈ�� ���� ��ġ�� ����(hitEffect�� hit�� �浹 �������� hit��...���� ���ͷ� ȸ����)
            effect.transform.parent = hit.transform;
            //effect�� �θ�� hit�� ��ġ��.
            Destroy(effect.gameObject, 3f); //effect�� �������� ������� ������ ��� ���̰� �ǹǷ� 3���� ���� �Ͽ� �����ش�.
            hittable?.Hit(hit, damage);//IHitable�� ���� �¾������ hit�� Ʈ���� ��� ����
        }
    }
    
}
