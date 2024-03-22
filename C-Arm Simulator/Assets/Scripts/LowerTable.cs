using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerTable : MonoBehaviour
{
    public GameObject buttonDown;
    public GameObject table;
    private Vector3 initialPosition;
    private bool isButtonDown = false;
    private float elapsedTime = 0f;
    private float duration = 1.0f; // Adjust the duration as needed
    private bool BeingPressed = false;
    public Shader redShader;
    private Shader normalColor;

    void Start()
    {
        initialPosition = table.transform.position;
        normalColor = buttonDown.GetComponent<Shader>();
    }

    void Update()
    {
        Vector3 currentPosition = table.transform.position;
        if (BeingPressed)
        {
            table.transform.position = new Vector3(currentPosition.x, currentPosition.y - 0.0003f, currentPosition.z);
            elapsedTime += Time.deltaTime;
        }
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
        BeingPressed = true;
        // buttonDown.GetComponent<Shader>() = redShader;
        //Debug.Log("A button has been pressed");
        if (elapsedTime < duration)
        {
            // table.transform.position = Vector3.Lerp(initialPosition, initialPosition + new Vector3(0, -0.3f, 0), elapsedTime / duration);
            // elapsedTime += Time.deltaTime;
        }
    }

    public void onButtonRelease()
    {
        BeingPressed = false;
        //  buttonDown.GetComponent<Shader>() = normalColor;
        // Debug.Log("A button has be let go");
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