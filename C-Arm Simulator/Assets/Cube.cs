using System;
using Unity.VisualScripting;
using UnityEditor.Scripting.Python;
using UnityEditor;
using UnityEngine;

public class Cube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RunPythonScripts();
        Debug.Log("Completed.");
    }
    static void RunPythonScripts()
    {
        //PythonRunner.RunFile($"{Application.dataPath}/ensure_naming.py");
        //python3 xraySimulation.py --angle_x 30 --angle_y 45 --angle_z 60
        
        // Debug.Log("Nifti To Array.");
        // PythonRunner.RunFile($"{Application.dataPath}/niftiToArray.py");
        // Debug.Log("Simple X-Ray Rotation");
        // PythonRunner.RunFile($"{Application.dataPath}/simplexrayRotation.py");
        
        // Debug.Log("Calling X-Ray Generator.");
        // PythonRunner.RunFile($"{Application.dataPath}/xrayGenerator.py");
        
         Debug.Log("Numbpy example:");
         PythonRunner.RunFile($"{Application.dataPath}/savetxt.py");
    }
}
