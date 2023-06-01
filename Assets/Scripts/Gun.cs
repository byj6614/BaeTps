using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] TrailRenderer bulletTraill;
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] int damage;
    public void Fire()
    {
        muzzleEffect.Play();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            IHitable hittable = hit.transform.GetComponent<IHitable>();
            ParticleSystem effect= Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            effect.transform.parent = hit.transform;
            Destroy(effect.gameObject, 3f);

            
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, hit.point));
            

            hittable?.Hit(hit, damage);

        }
        else
        {
            
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward*maxDistance));
        }
    }

    IEnumerator TrailRoutine(Vector3 startPoint,Vector3 endPoint)
    {
        TrailRenderer trail = Instantiate(bulletTraill, muzzleEffect.transform.position, Quaternion.identity);
        float totalTime=Vector2.Distance(startPoint, endPoint)/bulletSpeed;

        float rate = 0;
        while(rate<1)
        {
            trail.transform.position=Vector3.Lerp(startPoint, endPoint,rate);
            rate += Time.deltaTime / totalTime;

            yield return null;
        }

        Destroy(trail);
    }
}
