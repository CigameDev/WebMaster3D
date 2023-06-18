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
    int percision = 40;//so diem cua day
    float moveTime = 0f;
    float ropeProgressionSpeed = 10f;
    bool strightLine = false;
    public AnimationCurve cure;
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
        //lineRenderer.positionCount = percision;
        //LinePointsToFirePoint();
    }
    private void Update()
    {
        Vector3[] positions = new Vector3[]//vi tri cua hook va vi tri shoot
        {
            transform.position,
            grapple.shootTransform.transform.position
        };
        lineRenderer.SetPositions(positions);

        //moveTime += Time.deltaTime;
        //DrawRopeWaves();

    }
  
    public void MoveToTarget(Grapple grapple, Vector3 target)
    {
        if (stateHook != StateHook.MoveTarget) return;
        //DrawRopeWaves();
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * ropeProgressionSpeed);

        if (Vector3.Distance(transform.position, target) < 0.2f)
        {
            transform.position = target;
            stateHook = StateHook.Frozen;
            grapple.StartPull();
        }
    }
    
    public void LinePointsToFirePoint()
    {
        if (grapple == null) return;
        for(int i=0;i<percision;i++)
        {
            lineRenderer.SetPosition(i, grapple.shootTransform.transform.position);
        }    
    }    

    private void DrawRopeWaves()
    {
        if(grapple == null) return;
        for(int i =0;i< percision;i++)
        {
            float delta = (float)i / ((float)percision - 1f);
            Vector2 offset = Vector2.Perpendicular((Vector2)grapple._directionLine).normalized * cure.Evaluate(delta);
            Vector2 targetPosition = Vector2.Lerp(grapple.shootTransform.position, transform.position, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(grapple.shootTransform.position, targetPosition, cure.Evaluate(moveTime)*ropeProgressionSpeed);
            //lineRenderer.SetPosition(i, new Vector3(targetPosition.x,targetPosition.y,transform.position.z));
            lineRenderer.SetPosition(i, new Vector3(currentPosition.x,currentPosition.y,transform.position.z));
            //lineRenderer.SetPosition(i, currentPosition);
        }    
    }    
    private void DrawRopeNoWaves()
    {
        lineRenderer.SetPosition(0, grapple.shootTransform.transform.position);
        lineRenderer.SetPosition(1, this.transform.position);
    }    

    private void DrawRope()
    {
        if(!strightLine)
        {

        }    
    }    
}
