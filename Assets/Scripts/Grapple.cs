using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gan script nay cho player
/// </summary>
public class Grapple : MonoBehaviour
{
    [SerializeField] float pullSpeed = 0.5f;
    [SerializeField] float stopDistance = 1f;
    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform shootTransform;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 directionLine;
    Hook hook;
    bool pulling;
    Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        pulling = false;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPoint = GetMousePosition();
        }

        if (hook == null && Input.GetMouseButtonUp(0))
        {
            endPoint = GetMousePosition();
            directionLine = (endPoint - startPoint).normalized;

            StopAllCoroutines();
            pulling = false;
            hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
            hook.Inittialize(this, directionLine);
            StartCoroutine(DestroyHookAfterLifetime());
        }
        else if (hook != null && Input.GetMouseButtonDown(1))//xem lai doan nay,pha huy hook khi nhan chuot phai
        {
            DestroyHook();
        }

        //if (hook ==null && Input.GetMouseButtonDown(0))//ban to nhen,chua dung logic lam vi khong su dung cach nay
        //{
        //    StopAllCoroutines();
        //    pulling = false;
        //    hook = Instantiate(hookPrefab,shootTransform.position,Quaternion.identity).GetComponent<Hook>();
        //    hook.Inittialize(this, shootTransform);
        //    StartCoroutine(DestroyHookAfterLifetime());
        //}
        //else if(hook !=null && Input.GetMouseButtonDown(1))//xem lai doan nay,pha huy hook khi nhan chuot phai
        //{
        //    DestroyHook();
        //}

        if (!pulling || hook == null) return;
        rigid.velocity = directionLine * pullSpeed * 20;
        //if (Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)//neu khoang cach nho hon stopDistance pha huy cai moc luon 
        //{
        //    //DestroyHook();
        //    //pulling = false;
        //}
        //else
        //{
        //    //rigid.AddForce(directionLine*pullSpeed,ForceMode.VelocityChange);//dat van toc moi cho doi tuong ,khong quan trong van toc truoc day
        //     rigid.velocity = directionLine * pullSpeed * 20;
        //    //rigid.AddForce((hook.transform.position - transform.position).normalized*pullSpeed,ForceMode.VelocityChange);//dat van toc moi cho doi tuong ,khong quan trong van toc truoc day
        //    //rigid.AddForce((hook.transform.position - transform.position).normalized*20);//dat van toc moi cho doi tuong ,khong quan trong van toc truoc day
        //}
    }

    public void StartPull()
    {
        pulling = true;
        rigid.useGravity = true;
    }    
    public void EndPull()
    {
        pulling = false;
        rigid.useGravity = false;
        if (hook != null)
        {
            Destroy(hook.gameObject);
        }
        hook = null;
    }    
    private void DestroyHook()
    {
        if (hook == null) return;
        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
    }   
    
    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(2f);
        DestroyHook();
    }
    private Vector3 GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private void OnCollisionEnter(Collision col)
    {
        if ((LayerMask.GetMask("Grapple") & 1 << col.gameObject.layer) > 0)
        {
            //rigid.isKinematic = true;
            EndPull();
        }    
    }
}
