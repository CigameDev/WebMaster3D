using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour

{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float startYFollow;
    [SerializeField] private float endYFollow;

    private void LateUpdate()
    {
        if (this.transform.position.y >= startYFollow && this.transform.position.y <= endYFollow)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, this.transform.position.y, -10);
        }

    }
}
