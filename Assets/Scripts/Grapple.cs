using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gan script nay cho player
/// </summary>
public class Grapple : MonoBehaviour
{
    [SerializeField] float pullSpeed = 0.5f;
    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform _shootTransform;
    public Transform shootTransform => _shootTransform;
    [SerializeField] LayerMask grapple;
    [SerializeField] Camera cam;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 directionLine;
    public Vector3 _directionLine => directionLine;
    private float startPlayerY;
    private Vector3 hitPoint;
    public Vector3 _hitPoint => hitPoint;
    private string nameObject;
    Hook hook;
    bool pulling;
    
    Rigidbody rigid;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
        pulling = false;
        startPlayerY = transform.position.y;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPoint = GetMousePosition();
            DestroyHook();
        }

        if (hook == null && Input.GetMouseButtonUp(0))
        {
            endPoint = GetMousePosition();
            directionLine = (endPoint - startPoint).normalized;

            StopAllCoroutines();
            pulling = false;
            hook = Instantiate(hookPrefab, _shootTransform.position, Quaternion.identity).GetComponent<Hook>();

            //ban raycast de xac dinh diem bam dinh
            RaycastHit hit;
            if (Physics.Raycast(hook.transform.position, directionLine, out hit, Mathf.Infinity, grapple))
            {
                hitPoint = hit.point;
                hook.stateHook = Hook.StateHook.MoveTarget;
                nameObject = hit.collider.name;
            }
          
            hook.Inittialize(this);
        }
        if (hook != null)
        {
            hook.MoveToTarget(this,hitPoint);
        }
        if (!pulling || hook == null) return;
        rigid.velocity = directionLine * pullSpeed * 20;


    }
    private void LateUpdate()
    {
        float posCamY = transform.position.y - startPlayerY;
        cam.transform.position = new Vector3(cam.transform.position.x, posCamY, -10);
    }
    public void StartPull()
    {
        pulling = true;
        this.GetComponent<Collider>().isTrigger = true;
    }    
    public void EndPull()
    {
        this.GetComponent<Collider>().isTrigger = false;
        //pulling = false;
        //if (hook != null)
        //{
        //    Destroy(hook.gameObject);
        //}
        //hook = null;
        DestroyHook();
        rigid.velocity = Vector3.zero;
        nameObject = "";
    }    
    private void DestroyHook()
    {
        if (hook == null) return;
        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
    }   
    
   
    private Vector3 GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return cam.ScreenToWorldPoint(mousePos);
    }

  
    private void OnTriggerEnter(Collider other)
    {
        if ((LayerMask.GetMask("Grapple") & 1 << other.gameObject.layer) > 0 && other.gameObject.name == nameObject)
        {
            EndPull();
        }
    }
}
