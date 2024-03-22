using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Photon.Pun;

public class XrayButton : MonoBehaviour, IPunObservable
{
    public Camera xRayCamera;
    public GameObject xRayDisplay;
    private bool isCooldown = false;
    private GameObject spine;
    public GameObject patient;

    private void Start()
    {
        spine = GameObject.FindGameObjectWithTag("Spine");
        spine.SetActive(false);
    }

    public void GetXray()
    {
        if (isCooldown)
        {
            return;
        }

        StartCoroutine(Cooldown());

        //SaveCameraView();
        // Call the RPC instead of directly calling SaveCameraView
        GetComponent<PhotonView>().RPC("SaveCameraView", RpcTarget.All);
        Debug.Log("An X ray photo has been taken");
// #if UNITY_EDITOR
//         StartCoroutine(CheckImageFile());
// #endif
    }

    [PunRPC]
    void SaveCameraView()
    {
        spine.SetActive(true);
        spine.transform.position = patient.transform.position;
        
        //string filePath = "";
            
        // Create a new RenderTexture with a depth of 24
        RenderTexture screenTexture = new RenderTexture(Screen.width, Screen.height, 24);

        // Set the targetTexture of the camera to the new RenderTexture
        xRayCamera.targetTexture = screenTexture;

        // Set the active RenderTexture to the new RenderTexture
        RenderTexture.active = screenTexture;

        // Clear the RenderTexture
        GL.Clear(true, true, Color.clear);

        // Render the camera's view
        xRayCamera.Render();

        // Create a new Texture2D and read the pixels from the RenderTexture into it
        Texture2D renderedTexture = new Texture2D(Screen.width, Screen.height);
        renderedTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        // Reset the active RenderTexture
        RenderTexture.active = null;

        // Encode the Texture2D to a PNG format
        byte[] byteArray = renderedTexture.EncodeToPNG();

        // Get the current timestamp and append it to the filename
//         string timestamp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
//
// #if UNITY_EDITOR
//         filePath = Application.dataPath + "/Resources/xraycapture_" + timestamp + ".png";
//         System.IO.File.WriteAllBytes(filePath, byteArray);
//         UnityEditor.AssetDatabase.Refresh();
// #endif
// #if UNITY_WSA
         //filePath = Application.persistentDataPath + "/Resources/xraycapture_" + timestamp + ".png";
         
         // Set texture directly
         Texture2D tex = new Texture2D(2, 2);
         tex.LoadImage(byteArray);
         // Create a new material and set its main texture to the loaded texture
         Material mat = new Material(Shader.Find("Standard"));
         mat.mainTexture = tex;

         // Apply the new material to the plane's renderer
         xRayDisplay.GetComponent<Renderer>().material = mat;
         
         spine.SetActive(false);
// #endif
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(5); // Cooldown duration is 5 seconds
        isCooldown = false;
    }

    IEnumerator CheckImageFile()
    {
        string directoryPath = "";
        Texture2D tex = null;
        Material mat = null;
        
#if UNITY_EDITOR
        directoryPath = Application.dataPath + "/Resources/";
#endif
// #if UNITY_WSA
//          directoryPath = Application.persistentDataPath + "/";
// #endif
        System.IO.FileInfo latestFile = null;

        while (true)
        {
            // Get all files in the directory
            var files = System.IO.Directory.GetFiles(directoryPath, "xraycapture_*.png");

            // If there are no files, wait for 1 second before checking again
            if (files.Length == 0)
            {
                yield return new WaitForSeconds(1);
                continue;
            }

            // Sort the files by creation time in descending order
            latestFile = files.Select(f => new System.IO.FileInfo(f))
                .OrderByDescending(fi => fi.CreationTime)
                .First();

            // If the latest file exists, break the loop
            if (latestFile.Exists)
            {
                break;
            }

            yield return new WaitForSeconds(1);
        }

#if UNITY_EDITOR
        // Load the latest file as a texture
        string fileName = System.IO.Path.GetFileNameWithoutExtension(latestFile.Name);
        tex = Resources.Load<Texture2D>(fileName);

        // Create a new material and set its main texture to the loaded texture
        mat = new Material(Shader.Find("Standard"));
        mat.mainTexture = tex;

        // Apply the new material to the plane's renderer
        xRayDisplay.GetComponent<Renderer>().material = mat;

        Debug.Log("File found and material applied: " + latestFile.FullName);
#endif
// #if UNITY_WSA
//          // Load the latest file as a texture using UnityWebRequestTexture.GetTexture
//                  
//                  UnityWebRequest request = UnityWebRequestTexture.GetTexture("file://" + latestFile.FullName);
//                  yield return request.SendWebRequest();
//                  if (request.result != UnityWebRequest.Result.Success)
//                  {
//                      Debug.LogError(request.error);
//                      yield break;
//                  }
//                  tex = DownloadHandlerTexture.GetContent(request);
//          
//                  // Create a new material and set its main texture to the loaded texture
//                  mat = new Material(Shader.Find("Standard"));
//                  mat.mainTexture = tex;
//          
//                  // Apply the new material to the plane's renderer
//                  xRayDisplay.GetComponent<Renderer>().material = mat;
//          
//                  Debug.Log("File found and material applied: " + latestFile.FullName);
// #endif
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }
}