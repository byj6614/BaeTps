using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] Rig aimRig;
    [SerializeField] private float reloadTime;
    [SerializeField] WeaponHolder weaponHolder;
    private Animator ani;
    private bool isReloading;

    private void Awake()
    {
        ani= GetComponent<Animator>();
      
    }
    
    IEnumerator ReloadRoutine()
    {
        ani.SetTrigger("Reloading");
        isReloading = true;
        aimRig.weight = 0f;
        yield return new WaitForSeconds(reloadTime);
        isReloading= false;
        aimRig.weight= 1f;
    }
    private void OnReload()
    {
        if (isReloading)
            return;
        StartCoroutine(ReloadRoutine());
    }

    public void Fire()
    {
        ani.SetTrigger("Fire");
        weaponHolder.Fire();
    }
    private void OnFire()
    {
        if (isReloading)
            return;
        Fire();
    }
}
