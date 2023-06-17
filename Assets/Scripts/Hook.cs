using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// gan cho hookPrefab
/// </summary>
public class Hook : MonoBehaviour
{

    Grapple grapple;
    LineRenderer lineRenderer;

    public enum StateHook
    {
        None =0,
        MoveTarget,
        Frozen
    }    
    public StateHook stateHook = StateHook.None;
    public void Inittialize(Grapple grapple)
    {
        this.grapple = grapple;
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        Vector3[] positions = new Vector3[]//vi tri cua hook va vi tri shoot
        {
            transform.position,
            grapple._shoot.transform.position
        };
        lineRenderer.SetPositions(positions);

    }
  
    public void MoveToTarget(Grapple grapple, Vector3 target)
    {
        if (stateHook != StateHook.MoveTarget) return;

        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 10f);
        if (Vector3.Distance(transform.position, target) < 0.2f)
        {
            transform.position = target;
            stateHook = StateHook.Frozen;
            grapple.StartPull();
        }
    }
   
}
