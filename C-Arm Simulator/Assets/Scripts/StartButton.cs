using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{


    public GameObject iCarm;
    private Quaternion rot;
    private Vector3  pos;

    public GameObject iKnob;
    public Quaternion rotKnob;
    private Vector3 posKnob;

    public GameObject iKnobB;
    private Quaternion rotKnobB;
    private Vector3  posKonbB;
    
    // Start is called before the first frame update
    void Start()
    {
        pos = iCarm.transform.position;
         rot = iCarm.transform.rotation;

         posKnob = iKnob.transform.position;
         rotKnob = iKnob.transform.rotation;
         
         posKonbB = iKnobB.transform.position;
         rotKnobB = iKnobB.transform.rotation;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void resetPostion()
    {
        iKnobB.transform.SetPositionAndRotation(posKonbB,rotKnobB);
        iKnob.transform.SetPositionAndRotation(posKnob,rotKnob);
        iCarm.transform.SetPositionAndRotation(pos,rot); 
    }
}
