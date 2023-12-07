using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnobTwist : MonoBehaviour
{

    //[SerializeField] private Transform handle;
    public GameObject handle;
    private Vector3 initialPosition;
    private Vector3 mousePos;
    private Vector3 initialMousePosition;
    public float rotationSensitivity = 0.5f; 
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = handle.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag()
    {
       mousePos = Input.mousePosition;
       Vector2 dir = mousePos - handle.transform.position;
       float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
       Quaternion r = Quaternion.AngleAxis(angle *2, Vector3.up);
       handle.transform.rotation = r;
        
          Debug.Log(angle);  
    }
    
    
}
