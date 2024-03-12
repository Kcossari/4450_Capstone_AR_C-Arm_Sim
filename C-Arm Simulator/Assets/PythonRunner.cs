using System;
using Unity.VisualScripting;
using UnityEditor.Scripting.Python;
using UnityEditor;
using UnityEngine;

// public class HelloWorld : MonoBehaviour
// {
//     //[MenuItem("Python/Hello World")]
//
//     void Start()
//     {
//         Debug.Log("Call python: ");
//         PrintHelloWorldFromPython();
//     }
//     
//     static void PrintHelloWorldFromPython()
//     {
//         PythonRunner.RunString(@"
// import UnityEngine;
// UnityEngine.Debug.Log('hello world')
// ");
//     }
// }

public class EnsureNaming : MonoBehaviour
{
    // [MenuItem("Python/Ensure Naming")]

    private void Start()
    {
        Debug.Log("Call python file: ");
        RunEnsureNaming();
    }

    static void RunEnsureNaming()
    {
        //PythonRunner.RunFile($"{Application.dataPath}/ensure_naming.py");
        //python3 xraySimulation.py --angle_x 30 --angle_y 45 --angle_z 60
        
        PythonRunner.RunFile($"{Application.dataPath}/niftiToArray.py");
        PythonRunner.RunFile($"{Application.dataPath}/simplexrayRotation.py");
    }
}
