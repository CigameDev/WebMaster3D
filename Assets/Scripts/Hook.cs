using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// gan cho hookPrefab
/// </summary>
public class Hook : MonoBehaviour
{
    [SerializeField] float hookForce = 25f;

    Grapple grapple;
    Rigidbody rb;
    LineRenderer lineRenderer;

    //public void Inittialize(Grapple grapple,Transform shootTransform)
    //{
    //    transform.forward = shootTransform.forward;
    //    this.grapple = grapple;
    //    rb = GetComponent<Rigidbody>();
    //    lineRenderer = GetComponent<LineRenderer>();
    //    rb.AddForce(transform.forward*hookForce,ForceMode.Impulse);
    //}

    public void Inittialize(Grapple grapple, Vector3 directionLine)
    {
        //transform.forward = shootTransform.forward;
        this.grapple = grapple;
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        rb.AddForce(directionLine * hookForce, ForceMode.Impulse);
    }
    private void Update()
    {
        Vector3[] positions = new Vector3[]//vi tri cua hook va vi tri player
        {
            transform.position,
            grapple.transform.position
        };
        lineRenderer.SetPositions(positions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if((LayerMask.GetMask("Grapple") & 1<<other.gameObject.layer)>0)
        {
            Debug.Log("va cham voi tuong");
            rb.useGravity = false;
            rb.isKinematic = true;

            grapple.StartPull();
        }    
    }
}
