using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeGun : MonoBehaviour
{
    //레이케스트를 통한 hit 구현
    [SerializeField] float maxDistance; //총의 사정거리
    [SerializeField] int damage;        //총의 데미지
    //파티클을 통한 총구와 피격시 이펙트 구현
    [SerializeField] ParticleSystem hitEffect;      //총이 맞았을 때 이펙트 파티클
    [SerializeField] ParticleSystem muzzleEffect;   //총이 발사되었을 때 머즐에서 나오는 이펙트


    public void Fire()
    {
        muzzleEffect.Play();//머즐에서는 발사됨과 동시에 이펙트가 나와야 하므로 Fire 발동시 바로 실행
        RaycastHit hit;     //레이케스트 생성
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit,maxDistance))
        {   //메인카메라의위치에서 , 메임카메라의 앞으로 hit을 내보낸다 maxDistance까지
            IHitable hittable = hit.transform.GetComponent<IHitable>();
            ParticleSystem effect=Instantiate(hitEffect,hit.point,Quaternion.LookRotation(hit.normal));
            //파티클 시스템 이펙트 오브젝트를 옆의 위치에 생성(hitEffect를 hit의 충돌 지점에서 hit의...무슨 벡터로 회전함)
            effect.transform.parent = hit.transform;
            //effect의 부모는 hit의 위치다.
            Destroy(effect.gameObject, 3f); //effect가 생성된후 사라지지 않으면 계속 쌓이게 되므로 3초후 삭제 하여 없애준다.
            hittable?.Hit(hit, damage);//IHitable을 통해 맞았을경우 hit이 트루일 경우 실행
        }
    }
    
}
