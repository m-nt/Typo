using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class LineRendererFill : MonoBehaviour
{
    private LineRenderer Line;
    public Color fillColor;
    public bool Generate_mesh = false;

    // Update is called once per frame
    void Update()
    {
        if (Generate_mesh)
        {
            GeneratePentagonShape();
            Generate_mesh = false;
        }
    }
    public void GeneratePentagonShape()
    {
        if (Line == null) Line = GetComponent<LineRenderer>();
        // Define the points of the pentagon
        Vector3[] points = new Vector3[Line.positionCount];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Line.GetPosition(i);
        }

        // Create a new material for the fill color
        Material fillMaterial = new(Shader.Find("Sprites/Default"))
        {
            color = fillColor
        };

        // Create a new GameObject for the fill mesh
        GameObject fillMeshObject;
        MeshRenderer fillRenderer;
        MeshFilter fillFilter;
        if (transform.childCount > 0)
        {
            fillMeshObject = transform.GetChild(0).gameObject;
            // Attach a MeshRenderer and MeshFilter to the fill mesh GameObject
            fillRenderer = fillMeshObject.GetComponent<MeshRenderer>();
            fillFilter = fillMeshObject.GetComponent<MeshFilter>();
        }
        else
        {
            fillMeshObject = new("FillMesh");
            fillMeshObject.transform.parent = transform;
            fillMeshObject.transform.localPosition = Vector3.zero;
            // Attach a MeshRenderer and MeshFilter to the fill mesh GameObject
            fillRenderer = fillMeshObject.AddComponent<MeshRenderer>();
            fillFilter = fillMeshObject.AddComponent<MeshFilter>();
        }


        int[] triangles = new int[(Line.positionCount - 2) * 3];

        // Calculate triangle indices
        for (int i = 0; i < Line.positionCount - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        // Generate the mesh for the fill area
        Mesh fillMesh = new()
        {
            vertices = points,
            triangles = triangles,
        };
        fillMesh.RecalculateNormals();
        fillFilter.sharedMesh = fillMesh;

        // Assign the fill material to the fill mesh renderer
        fillRenderer.sharedMaterial = fillMaterial;
    }
}
