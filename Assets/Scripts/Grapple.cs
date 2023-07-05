using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Gan script nay cho player
/// </summary>
public class Grapple : MonoBehaviour
{
    [SerializeField] float pullSpeed = 10f;
    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform _shootTransform;
    public Transform shootTransform => _shootTransform;
    [SerializeField] LayerMask grapple;
    [SerializeField] Camera cam;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 directionLine;
    public Vector3 _directionLine => directionLine;
    private Vector3 hitPoint;
    public Vector3 _hitPoint => hitPoint;
    private string nameObject;
    private Vector3 sizePlayer;
    Hook hook;
    public bool pulling { get; set; }

    
    Rigidbody rigid;

    public enum GripDirection
    {
        None,
        Left ,
        Right,
        Down,
        Up
    }
    private GripDirection gripDir;
    private GripDirection oldGrip;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
        pulling = false;
        sizePlayer = GetComponent<BoxCollider>().bounds.size;
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
                GetGripDirection(hit);
                hook.stateHook = Hook.StateHook.MoveTarget;
                nameObject = hit.collider.name;
            }
          
            hook.Inittialize(this);
        }
        if (hook != null)
        {
            hook.MoveToTarget(this,hitPoint);
        }
        PullPlayer();
    }
  
    public void StartPull()
    {
        pulling = true;
        this.GetComponent<Collider>().isTrigger = true;
    }    
    public void EndPull()
    {
        this.GetComponent<Collider>().isTrigger = false;
        DestroyHook();
        rigid.velocity = Vector3.zero;
        nameObject = "";
        oldGrip = gripDir;
        gripDir = GripDirection.None;
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
            RotatePlayer();
            EndPull();
        }
    }

    // Rotate Player khi Player bam vao vi tri moi
    private void RotatePlayer()
    {
        if (gripDir == GripDirection.None) return;
        Vector3 newAngel = new Vector3(transform.rotation.x,transform.rotation.y,transform.rotation.z);

        if (gripDir == GripDirection.Left)
        {
            newAngel = new Vector3(newAngel.x, 135f, newAngel.z);
        }
        else if (gripDir == GripDirection.Right)
        {
            newAngel = new Vector3(newAngel.x, 315f, newAngel.z);
        }
        else if (gripDir == GripDirection.Down)
        {
            newAngel = new Vector3(newAngel.x, newAngel.y, 0f);
            if (oldGrip == GripDirection.Up)
            {
                this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
        }
        else if (gripDir == GripDirection.Up)
        {
            newAngel = new Vector3(newAngel.x, newAngel.y, 180f);
            this.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);

        }
        this.transform.rotation = Quaternion.Euler(newAngel);
    }

    //lay vi tri se bam tiep theo dua vao raycast
    private void GetGripDirection(RaycastHit hit)
    {
        if (hit.normal == new Vector3(1f, 0f, 0f))
        {
            gripDir = GripDirection.Left;
        }
        else if (hit.normal == new Vector3(-1f, 0f, 0f))
        {
            gripDir = GripDirection.Right;
        }
        else if (hit.normal == new Vector3(0f, -1f, 0f))
        {
            gripDir = GripDirection.Up;
        }
        else if (hit.normal == new Vector3(0f, 1f, 0f))
        {
            gripDir = GripDirection.Down;
        }
    }    

    //keo player ve hitpoint
    private void PullPlayer()
    {
        if (!pulling || hook == null) return;
        rigid.velocity = directionLine * pullSpeed;
    }    
}
