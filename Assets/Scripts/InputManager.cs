using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector3 directionLine {  get; private set; }
    private Vector3 startPoint;
    private Vector3 endPoint;


    public bool canShoot;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPoint = GetMousePosition();
        }
        if(Input.GetMouseButtonUp(0))
        {
            canShoot = true;
            endPoint = GetMousePosition();
            directionLine = endPoint - startPoint;
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }    
}
