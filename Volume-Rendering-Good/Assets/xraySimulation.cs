using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Globalization;

public class xraySimulation : MonoBehaviour
{
    // Assuming you've loaded your 3D array data into this variable
    private float[,,] voxelArray;


    void Start()
    {

        // LOOP THROUGH IMAGES
        string textFilePath = "cropped_output_3d_array.txt";
        voxelArray = Read3DArrayFromText(textFilePath);

        for (int angleDegrees = 0; angleDegrees <= 360; angleDegrees += 30)
        {
            var xrayImage = SimulateXrayWithRotation(voxelArray, angleDegrees);
            var mask = CreateCircularMask(xrayImage.GetLength(0), xrayImage.GetLength(1));
            var circularXrayImage = ApplyCircularMask(xrayImage, mask);

            // Save the circular-shaped X-ray as PNG with an angle in the filename
            SaveXrayToPng(circularXrayImage, $"xray_{angleDegrees}.png");
        }

        // CREATE ONE IMAGE
        // string textFilePath = "cropped_output_3d_array.txt";
        // voxelArray = Read3DArrayFromText(textFilePath);

        // int angleDegrees = 180; // Change as needed
        // var xrayImage = SimulateXrayWithRotation(voxelArray, angleDegrees);
        // var mask = CreateCircularMask(xrayImage.GetLength(0), xrayImage.GetLength(1));
        // var circularXrayImage = ApplyCircularMask(xrayImage, mask);

        // // Save the circular-shaped X-ray as PNG
        // SaveXrayToPng(circularXrayImage, "xray.png");
    }



    float[,,] Read3DArrayFromText(string filePath)
{
    List<List<List<float>>> slices = new List<List<List<float>>>();
    List<List<float>> currentSlice = new List<List<float>>();

    using (StreamReader reader = new StreamReader(filePath))
    {
        string line;
        bool readingSlice = false;

        Debug.Log($"Starting to read file: {filePath}");

        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith("# Array shape:")) continue;

            if (line.StartsWith("# New slice"))
            {
                if (readingSlice && currentSlice.Count > 0)
                {
                    slices.Add(new List<List<float>>(currentSlice));
                    currentSlice.Clear();
                }
                readingSlice = true;
            }
            else if (readingSlice)
            {
                if (currentSlice.Count % 100 == 0) {
                    Debug.Log($"Processed {currentSlice.Count} slices...");
                }
                string[] parts = line.Split();
                List<float> row = new List<float>();
                foreach (string part in line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrEmpty(part))
                    {
                        if (float.TryParse(part, NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
                        {
                            row.Add(result);
                        }
                        else
                        {
                            Debug.LogWarning($"Failed to parse '{part}' as float.");
                        }
                    }
                }
                currentSlice.Add(row);
            }
        }

        if (currentSlice.Count > 0)
        {
            slices.Add(new List<List<float>>(currentSlice));
        }
    }

    int depth = slices.Count;
    int height = slices[0].Count;
    int width = slices[0][0].Count;
    float[,,] array3D = new float[depth, height, width];

    for (int z = 0; z < depth; z++)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                array3D[z, y, x] = slices[z][y][x];
            }
        }
    }

    return array3D;
}


    // float[,] SimulateXrayWithRotation(float[,,] array, int angleDegrees)
    // {
    //     angleDegrees = angleDegrees % 360; // Normalize angle to [0, 360)
    //     int depth = array.GetLength(0);
    //     int height = array.GetLength(1);
    //     int width = array.GetLength(2);

    //     float[,] projectedImage;

    //     if (0 <= angleDegrees && angleDegrees < 90)
    //     {
    //         // Sum along the depth axis
    //         projectedImage = new float[height, width];
    //         for (int y = 0; y < height; y++)
    //         {
    //             for (int x = 0; x < width; x++)
    //             {
    //                 float sum = 0;
    //                 for (int z = 0; z < depth; z++)
    //                 {
    //                     sum += array[z, y, x];
    //                 }
    //                 projectedImage[y, x] = sum;
    //             }
    //         }
    //     }
    //     else if (90 <= angleDegrees && angleDegrees < 180)
    //     {
    //         // Sum along the width axis
    //         projectedImage = new float[depth, height];
    //         for (int z = 0; z < depth; z++)
    //         {
    //             for (int y = 0; y < height; y++)
    //             {
    //                 float sum = 0;
    //                 for (int x = 0; x < width; x++)
    //                 {
    //                     sum += array[z, y, x];
    //                 }
    //                 projectedImage[z, y] = sum;
    //             }
    //         }
    //     }
    //     else if (180 <= angleDegrees && angleDegrees < 270)
    //     {
    //         // Sum along the height axis
    //         projectedImage = new float[depth, width];
    //         for (int z = 0; z < depth; z++)
    //         {
    //             for (int x = 0; x < width; x++)
    //             {
    //                 float sum = 0;
    //                 for (int y = 0; y < height; y++)
    //                 {
    //                     sum += array[z, y, x];
    //                 }
    //                 projectedImage[z, x] = sum;
    //             }
    //         }
    //     }
    //     else // 270 <= angleDegrees < 360
    //     {
    //         // Sum along the width axis and flip horizontally
    //         projectedImage = new float[depth, height];
    //         for (int z = 0; z < depth; z++)
    //         {
    //             for (int y = 0; y < height; y++)
    //             {
    //                 float sum = 0;
    //                 for (int x = 0; x < width; x++)
    //                 {
    //                     sum += array[z, y, x];
    //                 }
    //                 projectedImage[z, height - 1 - y] = sum; // Flipping horizontally by mapping y to height - 1 - y
    //             }
    //         }
    //     }

    //     return projectedImage;
    // }

// -------------------------------
// ROTATE WITH SPECIFIED ANGLE
    // private float[,] SimulateXrayWithRotation(float[,,] voxelArray, int angleDegrees)
    // {
    //     float radians = angleDegrees * Mathf.Deg2Rad;
    //     int depth = voxelArray.GetLength(0);
    //     int height = voxelArray.GetLength(1);
    //     int width = voxelArray.GetLength(2);

    //     // Assuming projection along Z-axis after rotation
    //     float[,] projectedImage = new float[height, width];

    //     // Rotation around Y-axis
    //     float cosTheta = Mathf.Cos(radians);
    //     float sinTheta = Mathf.Sin(radians);

    //     for (int z = 0; z < depth; z++)
    //     {
    //         for (int y = 0; y < height; y++)
    //         {
    //             for (int x = 0; x < width; x++)
    //             {
    //                 // Calculate rotated coordinates
    //                 int newX = Mathf.FloorToInt(cosTheta * x - sinTheta * z);
    //                 int newZ = Mathf.FloorToInt(sinTheta * x + cosTheta * z);

    //                 // Check if the new coordinates are within bounds
    //                 if (newX >= 0 && newX < width && newZ >= 0 && newZ < depth)
    //                 {
    //                     // Sum projections
    //                     projectedImage[y, newX] += voxelArray[z, y, x];
    //                 }
    //             }
    //         }
    //     }

    //     return projectedImage;
    // }

    // -------------------------------
    private float[,] SimulateXrayWithRotation(float[,,] voxelArray, int angleDegrees)
{
    float radians = angleDegrees * Mathf.Deg2Rad;
    int depth = voxelArray.GetLength(0);
    int height = voxelArray.GetLength(1);
    int width = voxelArray.GetLength(2);

    float[,] projectedImage = new float[height, width];
    
    Vector3 center = new Vector3(width / 2f, height / 2f, depth / 2f);

    for (int z = 0; z < depth; z++)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Translate coordinates to rotate around the center
                Vector3 voxelPos = new Vector3(x, y, z) - center;

                // Apply rotation
                Vector3 rotatedPos = RotateX(voxelPos, radians) + center;

                // Map back to array indices and check bounds
                int newX = Mathf.FloorToInt(rotatedPos.x);
                int newY = Mathf.FloorToInt(rotatedPos.y);
                int newZ = Mathf.FloorToInt(rotatedPos.z);
                if (newX >= 0 && newX < width && newY >= 0 && newY < height && newZ >= 0 && newZ < depth)
                {
                    projectedImage[newY, newX] += voxelArray[z, y, x]; // Assuming projection along a certain axis
                }
            }
        }
    }

    return projectedImage;
}

private Vector3 RotateY(Vector3 point, float radians)
{
    float cosTheta = Mathf.Cos(radians);
    float sinTheta = Mathf.Sin(radians);
    return new Vector3(
        cosTheta * point.x + sinTheta * point.z,
        point.y,
        -sinTheta * point.x + cosTheta * point.z
    );
}

private Vector3 RotateX(Vector3 point, float radians)
{
    float cosTheta = Mathf.Cos(radians);
    float sinTheta = Mathf.Sin(radians);
    return new Vector3(
        point.x,
        cosTheta * point.y - sinTheta * point.z,
        sinTheta * point.y + cosTheta * point.z
    );
}

private Vector3 RotateZ(Vector3 point, float radians)
{
    float cosTheta = Mathf.Cos(radians);
    float sinTheta = Mathf.Sin(radians);
    return new Vector3(
        cosTheta * point.x - sinTheta * point.y,
        sinTheta * point.x + cosTheta * point.y,
        point.z
    );
}



    bool[,] CreateCircularMask(int rows, int cols, Vector2? center = null, float? radius = null)
    {
        bool[,] mask = new bool[rows, cols];
        Vector2 _center = center ?? new Vector2(cols / 2, rows / 2);
        float _radius = radius ?? Mathf.Min(rows, cols) / 2f;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                float distFromCenter = Vector2.Distance(new Vector2(x, y), _center);
                mask[y, x] = distFromCenter <= _radius;
            }
        }

        return mask;
    }


    float[,] ApplyCircularMask(float[,] image, bool[,] mask)
    {
        int rows = image.GetLength(0);
        int cols = image.GetLength(1);

        // Initialize a new image array to hold the result
        float[,] maskedImage = new float[rows, cols];

        // Iterate through each pixel in the image
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                // Check if the current pixel is within the circular mask
                if (mask[y, x])
                {
                    // If inside the mask, copy the original pixel value
                    maskedImage[y, x] = image[y, x];
                }
                else
                {
                    // If outside the mask, set the pixel value to 0 (or any other background value)
                    maskedImage[y, x] = 0;
                }
            }
        }

        return maskedImage;
    }


    void SaveXrayToPng(float[,] xrayImage, string fileName)
{
    int width = xrayImage.GetLength(1);
    int height = xrayImage.GetLength(0);
    Texture2D texture = new Texture2D(width, height);

    // Find actual min and max values in the xrayImage
    float minValue = float.MaxValue, maxValue = float.MinValue;
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            minValue = Mathf.Min(minValue, xrayImage[y, x]);
            maxValue = Mathf.Max(maxValue, xrayImage[y, x]);
        }
    }

    // Use the actual min and max for normalization
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            float normalizedValue = Mathf.InverseLerp(minValue, maxValue, xrayImage[y, x]);
            Color color = new Color(normalizedValue, normalizedValue, normalizedValue, 1.0f);
            texture.SetPixel(x, height - y - 1, color);
        }
    }

    texture.Apply();
    byte[] bytes = texture.EncodeToPNG();

    // MANUALLY SPECIFIED DIRECTORY
    string homePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    string customDirectory = Path.Combine(homePath, "Desktop", "UnitySavedImages");

    if (!Directory.Exists(customDirectory))
    {
        Directory.CreateDirectory(customDirectory);
    }

    string fullPath = Path.Combine(customDirectory, fileName);
    try
    {
        File.WriteAllBytes(fullPath, bytes);
        Debug.Log($"Successfully saved PNG to {fullPath}");
    }

    // UNITY DIRECTORY
    // Save the PNG to disk
    // string fullPath = Path.Combine(Application.persistentDataPath, fileName);
    // File.WriteAllBytes(fullPath, bytes);
    // Debug.Log($"X-ray image saved as PNG to {fullPath}");

    // TRYING TO SAVE TO UNITY ASSET FOLDER
    // When running in the Unity Editor, save to the Assets folder
    // string assetsSubDirectory = "GeneratedImages"; // Subdirectory under Assets for organization
    // string directoryPath = Path.Combine(Application.dataPath, assetsSubDirectory);

    // Check if the directory exists, if not, create it
    // if (!Directory.Exists(directoryPath))
    // {
    //     Directory.CreateDirectory(directoryPath);
    // }

    // string fullPath = Path.Combine(directoryPath, fileName);
    // try
    // {
    //     File.WriteAllBytes(fullPath, bytes);
    //     Debug.Log($"Successfully saved PNG to {fullPath}");

    //     // Refresh the AssetDatabase to show the new file in the Unity Editor
    //     UnityEditor.AssetDatabase.Refresh();
    // }
    // catch (Exception ex)
    // {
    //     Debug.LogError($"Failed to save PNG: {ex.Message}");
    // }

    catch (Exception ex)
    {
        Debug.LogError($"Failed to save PNG: {ex.Message}");
    }
}


}
