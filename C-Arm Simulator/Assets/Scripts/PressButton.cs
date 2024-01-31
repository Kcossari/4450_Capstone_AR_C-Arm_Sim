using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PressButton : MonoBehaviour
{
      
    public GameObject buttonDown;
    public GameObject table;
    private Vector3 initialPosition;
    private bool isButtonDown = false;
    private float elapsedTime = 0f;
    private float duration = 1.0f; // Adjust the duration as needed

    void Start()
    {
        initialPosition = table.transform.position;
        
        
    }

    void Update()
    {
   //     if (isButtonDown)
   //     {
   //         MoveButtonDown();
  //      }
    }

    private void OnMouseDown()
    {
    //    isButtonDown = true;
   //     elapsedTime = 0f;
    }

    private void MoveButtonDown()
    {
        /*
        if (elapsedTime < duration)
       {
          buttonDown.transform.position = Vector3.Lerp(initialPosition, initialPosition + new Vector3(0, -0.3f, 0), elapsedTime / duration);
           elapsedTime += Time.deltaTime;
        }
   
        else
        {
            
            // Reset the button position to its initial position
            float normalizedTime = (elapsedTime - (2 * duration + 0.5f)) / duration;
            buttonDown.transform.position = Vector3.Lerp(initialPosition + new Vector3(0, -0.3f, 0), initialPosition, normalizedTime);

            // Continue to update elapsedTime even after reaching the top
            elapsedTime += Time.deltaTime;

            // Reset the state after completing the upward movement
            if (normalizedTime >= 1.0f)
            {
                isButtonDown = false;
            }
        }
        */
    }

    public void onButtonGrab()
    {
        Debug.Log("A button has been pressed");
        if (elapsedTime < duration)
        {
            table.transform.position = Vector3.Lerp(initialPosition, initialPosition + new Vector3(0, -0.3f, 0), elapsedTime / duration);
            elapsedTime += Time.deltaTime;
        }
    }
    public void onButtonRelease()
    {
        Debug.Log("A button has be let go");
        
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("button is being pressed");
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("button no longer being pressed");
    }
}
