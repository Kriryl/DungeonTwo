using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CameraWarp is used to move the camera around.
/// </summary>
public class CameraWarp : MonoBehaviour
{
    public static List<Vector2> warps = new List<Vector2>();

    public static Camera Create(Vector2 coordinates, Camera cam)
    {
        float x = coordinates.x;
        float y = coordinates.y;

        Vector3 newCamPos = new Vector3(x, y, -10f);

        cam.transform.position = newCamPos;
        warps.Add(newCamPos);
        return cam;
    }
}
