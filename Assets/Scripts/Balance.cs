using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    public GameObject body;
    private void Update()
    {
        transform.position = body.transform.position;
    }
}
