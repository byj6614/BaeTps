using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IHitable
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Hit(RaycastHit hit,int damage)
    {
       rb?.AddForceAtPosition(-10*hit.normal,hit.point,ForceMode.Impulse);
    }
}
