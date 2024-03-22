using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableXMovment : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject table;
    public GameObject arch;
    private float initialXPositionObject2;
    private float initialtablePostion;
    void Start()
    {
        if (arch != null)
        {
            // Store the initial X position of object2
            initialXPositionObject2 = arch.transform.position.x;
        }
        if (table != null)
        {
            // Store the initial X position of object2
            initialtablePostion= table.transform.position.z;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (table != null && arch != null)
        {
            // Get the current position of object1
            Vector3 currentPosition = table.transform.position;

            // Set the X position of object1 to match the Y position of object2
            currentPosition.z =   currentPosition.z   + (arch.transform.position.x - initialXPositionObject2) ;
            
            // Apply the updated position to object1
            if (currentPosition.z > 6f)
            {
                currentPosition.z = 6f;
                table.transform.position = currentPosition;
            }else if (currentPosition.z < 4f)
            {
                currentPosition.z = 4f;
                table.transform.position = currentPosition;
            }
            else
            {
                table.transform.position = currentPosition;    
            }
            
        }
    }
    }

