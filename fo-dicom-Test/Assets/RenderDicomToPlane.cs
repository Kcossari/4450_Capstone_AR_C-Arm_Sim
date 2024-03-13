using UnityEngine;
using FellowOakDicom.Imaging;

public class RenderDicomToPlane : MonoBehaviour
{
    void Start()
    {
        // Replace "image.0001.dcm" with the correct file name
        var dicomImage = new DicomImage(@"Assets/DICOM/image.0001.dcm");
        var texture = dicomImage.RenderImage().AsTexture2D();

        // Apply the texture to the plane's material
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = texture;
    }
}
