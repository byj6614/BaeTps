using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeWork
{
    public class HomeGun : MonoBehaviour
    {
        //�����ɽ�Ʈ�� ���� hit ����
        [SerializeField] float maxDistance; //���� �����Ÿ�
        [SerializeField] int damage;        //���� ������
                                            //��ƼŬ�� ���� �ѱ��� �ǰݽ� ����Ʈ ����
        [SerializeField] ParticleSystem hitEffect;      //���� �¾��� �� ����Ʈ ��ƼŬ
        [SerializeField] ParticleSystem muzzleEffect;   //���� �߻�Ǿ��� �� ���񿡼� ������ ����Ʈ
                                                        //Ʈ������ ���� �Ѿ� ���� ����
        [SerializeField] TrailRenderer bulletTrail;
        [SerializeField] float bulletSpeed;

        public void Fire()
        {
            muzzleEffect.Play();//���񿡼��� �߻�ʰ� ���ÿ� ����Ʈ�� ���;� �ϹǷ� Fire �ߵ��� �ٷ� ����
            RaycastHit hit;     //�����ɽ�Ʈ ����
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
            {   //����ī�޶�����ġ���� , ����ī�޶��� ������ hit�� �������� maxDistance����
                IHitable hittable = hit.transform.GetComponent<IHitable>();
                ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                //��ƼŬ �ý��� ����Ʈ ������Ʈ�� ���� ��ġ�� ����(hitEffect�� hit�� �浹 �������� hit��...���� ���ͷ� ȸ����)
                effect.transform.parent = hit.transform;
                //effect�� �θ�� hit�� ��ġ��.
                Destroy(effect.gameObject, 3f); //effect�� �������� ������� ������ ��� ���̰� �ǹǷ� 3���� ���� �Ͽ� �����ش�.
                hittable?.Hit(hit, damage);//IHitable�� ���� �¾������ hit�� Ʈ���� ��� ����


            }
        }

        //�Ѿ˱����� �ڸ�ƾ�� �̿��Ͽ� ������ش�.
        IEnumerator TrailRoutine(Vector3 startPoint,Vector3 endPoint)//�Ѿ� ������ ������ ��ġ�� ������ ��ġ�� �̸� �������ش�.
        {
            TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
            float totalTime=Vector2.Distance(startPoint, endPoint)/bulletSpeed; //�ѽð��� �Ÿ�/�ӵ��� ������ش�.

            float rate = 0;//�� ���� ���Ÿ� ���� ���� �� ����
            while(rate<1)
            {
                trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate);//���� ��ġ���� ���Ÿ� ������ ������� ǥ��
                rate += Time.deltaTime / totalTime;//rate�� deltime��ŭ ��� ���� ��Ų�ٰ� ���� �ȴ�.

                yield return null;
            }
            Destroy(trail);//���� ������ ���� ȿ���� ���� ����� ������� ���ش�.
        }
    }
}