using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody[] _ragdollRigidbodies;

    private void Awake()
    {
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EnableRagdoll();
        }    
    }
    private void DisableRagdoll()
    {
        foreach(var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }    

    private void EnableRagdoll()
    {
        foreach(var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }    
    }    
}
