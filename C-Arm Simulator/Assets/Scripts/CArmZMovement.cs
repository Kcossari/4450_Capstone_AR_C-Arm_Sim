using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CArmZMovement : MonoBehaviour
{
    public GameObject cArm;
    private Vector3 initialPosition;

    private bool moveLeft = false;

    private bool moveRight = false;
// Start is called before the first frame update
    void Start()
    {
        initialPosition = cArm.transform.position;
    }

// Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = cArm.transform.position;
        if (moveLeft)
        {
   
           // if ((initialPosition.x - 0.125f) < currentPosition.x)
           // {
                cArm.transform.position = new Vector3(currentPosition.x -0.0003f, currentPosition.y , currentPosition.z);          
           // }
              
        }else if (moveRight)
        {
            //    if((initialPosition.x +0.125f) > currentPosition.x)
            cArm.transform.position = new Vector3(currentPosition.x + 0.0003f, currentPosition.y, currentPosition.z);
            //   }
        }

    }


    public void onButtonGrabIn()
    {
        moveLeft = true;
    }

    public void onReleaseButtonIn()
    {
        moveLeft = false;
    }


    public void onButtonGrabOut()
    {
        moveRight = true;
    }

    public void onReleaseButtonOut()
    {
        moveRight = false;
    }
}
