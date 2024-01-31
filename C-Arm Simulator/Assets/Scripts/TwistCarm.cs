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
    void Start()
    {
        if (knob != null)
        {
            // Store the initial X position of object2
            initialXPositionObject2 = knob.transform.rotation.y;
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (knob != null && cArm != null)
        {
            Quaternion currentRotation = cArm.transform.rotation;
            // Set the rotation of object1's Transform component
            // to match the y rotation of object2's Transform component
            
            // need a function that will fluently map the knobs y roation to the c arms x rotation
       
           // float rot = cArm.transform.rotation.x  + (knob.transform.eulerAngles.y - initialXPositionObject2);
         //  float rot = cArm.transform.rotation.x + (knob.transform.position.y - initialXPositionObject2);

         Quaternion rot = knob.transform.rotation;

           Debug.Log(rot);
           
       
      

            //no idea why is is not working
            
           Quaternion newRotation = Quaternion.Euler(rot.eulerAngles.y+90f, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z);
          

          cArm.transform.rotation = newRotation;
          //cArm.transform.rotation.x 
        }
    }
}
