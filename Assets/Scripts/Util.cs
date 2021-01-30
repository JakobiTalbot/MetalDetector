using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    /// <summary>
    /// Returns a point on a bezier curve using four control points
    /// </summary>
    /// <param name="t">Percentage of the way through the curve, from 0..1</param>
    /// <param name="c1">Control point 1</param>
    /// <param name="c2">Control point 2</param>
    /// <param name="c3">Control point 3</param>
    /// <param name="c4">Control point 4</param>
    /// <returns>A point somewhere along the curve</returns>
    public static Vector3 Bezier(float t, Vector3 c1, Vector3 c2, Vector3 c3, Vector3 c4)
    { return (c1 * B1(t)) + (c2 * B2(t)) + (c3 * B3(t)) + (c4 * B4(t)); }

    // bad bezier curve stuff
    static float B1(float t) { return t * t * t; }
    static float B2(float t) { return 3.0f * t * t * (1.0f - t); }
    static float B3(float t) { return 3.0f * t * (1.0f - t) * (1.0f - t); }
    static float B4(float t) { return (1.0f - t) * (1.0f - t) * (1.0f - t); }

    static Shader toonShader = null;
    public static void SetShader(GameObject obj)
    {
        if (toonShader == null)
            toonShader = Shader.Find("Toons/Toon Lite");
        if(toonShader == null)
        {
            Debug.LogError("Shader couldn't be found!");
            return;
        }

        MeshRenderer mr = obj.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            foreach(Material m in mr.materials)
            {
                m.shader = toonShader;
            }
        }

        // go through all children
        foreach (Transform t in obj.transform)
        {
            // check if this child has a meshrenderer
            SetShader(t.gameObject);
        }

    }

}
