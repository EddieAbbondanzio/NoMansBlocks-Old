using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDebugger : MonoBehaviour {
    public static VisualDebugger Instance;

    /// <summary>
    /// Color of the gizmos to render.
    /// </summary>
    public Color32 Color { get; set; }

    /// <summary>
    /// Radius of the points to render.
    /// </summary>
    public float SphereRadius { get; set; }

    /// <summary>
    /// Collection of points that are to be rendered as spheres.
    /// </summary>
    private List<Vector3> Points { get; set; }


    void Awake() {
        Instance = this;

        Points = new List<Vector3>();
        Color = new Color32(127, 0, 255, 255);
        SphereRadius = 0.25f;
    }

    void OnDrawGizmos() {
        if(Points != null) {
            Points.ForEach(p => Gizmos.DrawSphere(p, SphereRadius));
        }
    }

    /// <summary>
    /// A point to be drawn as a gizmo sphere.
    /// </summary>
    public void DrawPoint(Vector3 point) {
        Points.Add(point);
    }

    /// <summary>
    /// Checks if a point exists within the list. If it does the 
    /// point is then removed.
    /// </summary>
    public bool RemovePoint(Vector3 point) {
        return Points.Remove(point);
    }

    /// <summary>
    /// Erase the entire list of points to be rendered.
    /// </summary>
    public void ClearPoints() {
        Points.Clear();
    }

}
