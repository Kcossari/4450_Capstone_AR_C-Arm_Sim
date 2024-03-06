using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLRCarm : MonoBehaviour
{
    
    
    public GameObject knob;
    public GameObject cArm;
    private float initialXPositionObject2;
    private float initialCarm;
    // Start is called before the first frame update
    void Start()
    {
        
        if (knob != null)
        {
           
            initialXPositionObject2 = knob.transform.rotation.y;
            initialCarm = cArm.transform.eulerAngles.z;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (knob != null && cArm != null)
        {
            float knobRotation = knob.transform.rotation.y;
            float desiredZRotation = (knobRotation - initialXPositionObject2) * 100;
            
            Debug.Log(knobRotation + " " + desiredZRotation);

            /*
            if (desiredXRotation <= -45f)
            {
                desiredXRotation = -45f;
            }
            if (desiredXRotation >= 45f)
            {
                desiredXRotation = 45f;
            }
          */

            desiredZRotation = desiredZRotation + 180f;

            cArm.transform.eulerAngles = new Vector3(0f, 0f, desiredZRotation);

        }
    }
}
