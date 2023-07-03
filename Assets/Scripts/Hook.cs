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
    float waveSize = 0f;
    float startWaveSize = 2f;
    float straightenLineSpeed = 5;
    public AnimationCurve cure;
    public AnimationCurve ropeProgressionCurve;
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
        lineRenderer.positionCount = percision;
        waveSize = startWaveSize;
    }
    private void Update()
    {
        moveTime += Time.deltaTime;
        DrawRope();
    }
  
    public void MoveToTarget(Grapple grapple, Vector3 target)
    {
        if (stateHook != StateHook.MoveTarget) return;
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * ropeProgressionSpeed);

        if (Vector3.Distance(transform.position, target) < 0.2f)
        {
            transform.position = target;
            stateHook = StateHook.None;
            grapple.StartPull();
        }
    }
    
    private void DrawRopeWaves()
    {
        if(grapple == null) return;
        for(int i =0;i< percision;i++)
        {
            float delta = (float)i / ((float)percision - 1f);
            Vector2 offset = Vector2.Perpendicular((Vector2)grapple._directionLine).normalized * cure.Evaluate(delta) *waveSize;
            Vector2 targetPosition = Vector2.Lerp(grapple.shootTransform.position, transform.position, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(grapple.shootTransform.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime)*ropeProgressionSpeed);

            lineRenderer.SetPosition(i, new Vector3(targetPosition.x,targetPosition.y,transform.position.z));
            //lineRenderer.SetPosition(i, new Vector3(currentPosition.x,currentPosition.y,transform.position.z));
        }    
    }    
    private void DrawRopeNoWaves()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, grapple.shootTransform.transform.position);
        lineRenderer.SetPosition(1, this.transform.position);
    }    

    private void DrawRope()
    {

        if(stateHook == StateHook.None)
        {
            DrawRopeNoWaves();
        }   
        else
        {
            DrawRopeWaves();
        }    
    }

    //void DrawRope1()
    //{
    //    if (stateHook == StateHook.None)
    //    {
    //        if (lineRenderer.GetPosition(percision - 1).x == grapplingGun.grapplePoint.x)
    //        {
    //            stateHook = StateHook.Frozen;
    //        }
    //        else
    //        {
    //            DrawRopeWaves();
    //        }
    //    }
    //    else
    //    {
    //        if (!isGrappling)
    //        {
    //            grapplingGun.Grapple();
    //            isGrappling = true;
    //        }
    //        if (waveSize > 0)
    //        {
    //            waveSize -= Time.deltaTime * straightenLineSpeed;
    //            DrawRopeWaves();
    //        }
    //        else
    //        {
    //            waveSize = 0;

    //            if (lineRenderer.positionCount != 2) { lineRenderer.positionCount = 2; }

    //            DrawRopeNoWaves();
    //        }
    //    }
    //}
}
