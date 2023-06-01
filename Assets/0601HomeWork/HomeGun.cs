using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeWork
{
    public class HomeGun : MonoBehaviour
    {
        //레이케스트를 통한 hit 구현
        [SerializeField] float maxDistance; //총의 사정거리
        [SerializeField] int damage;        //총의 데미지
                                            //파티클을 통한 총구와 피격시 이펙트 구현
        [SerializeField] ParticleSystem hitEffect;      //총이 맞았을 때 이펙트 파티클
        [SerializeField] ParticleSystem muzzleEffect;   //총이 발사되었을 때 머즐에서 나오는 이펙트
                                                        //트레일을 통한 총알 궤적 생성
        [SerializeField] TrailRenderer bulletTrail;
        [SerializeField] float bulletSpeed;

        public void Fire()
        {
            muzzleEffect.Play();//머즐에서는 발사됨과 동시에 이펙트가 나와야 하므로 Fire 발동시 바로 실행
            RaycastHit hit;     //레이케스트 생성
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
            {   //메인카메라의위치에서 , 메임카메라의 앞으로 hit을 내보낸다 maxDistance까지
                IHitable hittable = hit.transform.GetComponent<IHitable>();
                ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                //파티클 시스템 이펙트 오브젝트를 옆의 위치에 생성(hitEffect를 hit의 충돌 지점에서 hit의...무슨 벡터로 회전함)
                effect.transform.parent = hit.transform;
                //effect의 부모는 hit의 위치다.
                Destroy(effect.gameObject, 3f); //effect가 생성된후 사라지지 않으면 계속 쌓이게 되므로 3초후 삭제 하여 없애준다.
                hittable?.Hit(hit, damage);//IHitable을 통해 맞았을경우 hit이 트루일 경우 실행


            }
        }

        //총알궤적은 코르틴을 이용하여 만들어준다.
        IEnumerator TrailRoutine(Vector3 startPoint,Vector3 endPoint)//총알 궤적이 시작할 위치와 도달할 위치를 미리 지정해준다.
        {
            TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
            float totalTime=Vector2.Distance(startPoint, endPoint)/bulletSpeed; //총시간은 거리/속도로 계산해준다.

            float rate = 0;//이 총이 끝거리 까지 닿을 때 까지
            while(rate<1)
            {
                trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate);//시작 위치에서 끝거리 까지를 백분율로 표현
                rate += Time.deltaTime / totalTime;//rate를 deltime만큼 계속 증가 시킨다고 보면 된다.

                yield return null;
            }
            Destroy(trail);//총의 궤적도 위에 효과와 같이 생기고서 사라지게 해준다.
        }
    }
}