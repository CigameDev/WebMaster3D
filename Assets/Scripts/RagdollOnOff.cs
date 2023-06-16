using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollOnOff : MonoBehaviour
{
    public BoxCollider mainCollider;
    public GameObject thisGuyRig;//lay cai skeletonOutlaw-Rig
    public Animator thisGuyAnimator;
    Collider[] ragDollColliders;
    Rigidbody[] limbsRigidbodies;
    private void Start()
    {
        GetRagdollBits();
        RagdollModeOff();
    }
    private void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //trong vi du nay this guy va cham voi vien da hay vien dan ,chuyen trang thai thanh ragdollModeOn
        if(collision.gameObject.tag =="Bullet")
        {
            RagdollModeOn();
        }    
    }
   
    private void GetRagdollBits()
    {
        ragDollColliders = thisGuyRig.GetComponentsInChildren<Collider>();
        limbsRigidbodies = thisGuyRig.GetComponentsInChildren<Rigidbody>();
    }    
    private void RagdollModeOn()
    {
        thisGuyAnimator.enabled = false;

        foreach (Collider col in ragDollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = false;
        }
        mainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }    
    private void RagdollModeOff()
    {
        foreach(Collider col in ragDollColliders)
        {
            col.enabled = false;
        }    
        foreach(Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = true;
        }    
        //thisGuyAnimator.enabled = true;
        //mainCollider.enabled = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<Animator>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }    
}
