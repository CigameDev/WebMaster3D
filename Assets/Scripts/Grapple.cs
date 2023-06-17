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
    [SerializeField] Camera cam;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 directionLine;
    private float startPlayerY;
    private bool canStopPull;
    Hook hook;
    bool pulling;
    Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        pulling = false;
        startPlayerY = transform.position.y;
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

        //if(Vector3.Distance(transform.position,hook.transform.position) <= stopDistance  )
        //{
        //    EndPull();
        //}
       
    }
    private void LateUpdate()
    {
        float posCamY = transform.position.y - startPlayerY;
        cam.transform.position = new Vector3(cam.transform.position.x, posCamY, -10);
    }
    public void StartPull()
    {
        pulling = true;
        rigid.useGravity = true;
    }    
    public void EndPull()
    {
        //this.transform.position = hook.transform.position;
        pulling = false;
        rigid.useGravity = false;
        if (hook != null)
        {
            Destroy(hook.gameObject);
        }
        hook = null;
        rigid.velocity = Vector3.zero;
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
        return cam.ScreenToWorldPoint(mousePos);
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
