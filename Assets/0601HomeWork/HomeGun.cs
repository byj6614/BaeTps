using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeGun : MonoBehaviour
{
    [SerializeField] float maxDistance; //총의 사정거리
    [SerializeField] int damage;

    public void Fire()
    {
        RaycastHit hit;     //레이케스트 생성
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit,maxDistance))
        {   //메인카메라의위치에서 , 메임카메라의 앞으로 hit을 내보낸다 maxDistance까지
            IHitable hittable = hit.transform.GetComponent<IHitable>();

            hittable?.Hit(hit, damage);//IHitable을 통해 맞았을경우 hit이 트루일 경우 실행
        }
    }
    
}
