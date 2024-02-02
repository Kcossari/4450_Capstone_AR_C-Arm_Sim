using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

public class TwistCarm : MonoBehaviour
{
    public GameObject knob;
    public GameObject cArm;
    private float initialXPositionObject2;
    private float initialCarm;
    void Start()
    {
        if (knob != null)
        {
            // Store the initial X position of object2
            initialXPositionObject2 = knob.transform.eulerAngles.y;
            initialCarm = cArm.transform.eulerAngles.x;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (knob != null && cArm != null)
        {
          //  Quaternion currentRotation = cArm.transform.rotation;
            // Set the rotation of object1's Transform component
            // to match the y rotation of object2's Transform component
           
            float knobRotation = knob.transform.eulerAngles.y;

            // Calculate the desired x rotation for the cArm
         
            float desiredXRotation = (knobRotation - initialXPositionObject2);
            //float desiredXRotation = knobRotation;

            if (desiredXRotation <= -45f)
            {
                desiredXRotation = -45f;
            } 
            if (desiredXRotation >= 45f)
            {
                desiredXRotation = 45f;
            }
          
            
            // Apply the rotation to the cArm
            //cArm.transform.rotation = Quaternion.Euler(desiredXRotation, 0f, 0f);
            ///desiredXRotation = desiredXRotation + 90f;

            desiredXRotation = desiredXRotation + 180f;
            Debug.Log(desiredXRotation);
            
           
            
           cArm.transform.eulerAngles = new Vector3(desiredXRotation, 0f, 0f);
           
            // need a function that will fluently map the knobs y roation to the c arms x rotation
            
           //float  rot = cArm.transform.rotation.x  + (kno- initialXPositionObject2);
         //  float rot = cArm.transform.rotation.x + (knob.transform.position.y - initialXPositionObject2);

        // Quaternion rot = knob.transform.rotation;

         //  Debug.Log(rot);
           
       
      

            //no idea why is is not working
            
          // Quaternion newRotation = Quaternion.Euler(currentRotation.eulerAngles.y+rot-269f, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z);
          

         // cArm.transform.rotation = newRotation;
          //cArm.transform.rotation.x 
        }
    }
}
