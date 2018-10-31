using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class ProceduralGrid : MonoBehaviour {

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    [Header("Grid Settings")]
    public float cellSize = 1;
    public Vector3 gridOffset;
    public int gridSize = 3;

    // Use this for initialization
    void Awake () {
        mesh = GetComponent<MeshFilter>().mesh;
	}

    void Start()
    {
        MakeContiguousProceduralGrid();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    // Discrete = Hard edges = No sharing of vertices between quads
    private void MakeDiscreteProceduralGrid()
    {
        // Set array size
        vertices = new Vector3[gridSize * gridSize * 4];
        triangles = new int[gridSize * gridSize * 6];

        // Set tracker integers
        int v = 0;
        int t = 0;

        // Set vertex offset
        float vertexOffset = cellSize * 0.5f;

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 cellOffset = new Vector3(x * cellSize, 0, z * cellSize);

                // Populate vertices and triangles
                vertices[v] = new Vector3(-vertexOffset, 0, -vertexOffset) + cellOffset + gridOffset;
                vertices[v + 1] = new Vector3(-vertexOffset, 0, vertexOffset) + cellOffset + gridOffset;
                vertices[v + 2] = new Vector3(vertexOffset, 0, -vertexOffset) + cellOffset + gridOffset;
                vertices[v + 3] = new Vector3(vertexOffset, 0, vertexOffset) + cellOffset + gridOffset;

                triangles[t] = v;
                triangles[t + 1] = triangles[t + 4] = v + 1;
                triangles[t + 2] = triangles[t + 3] = v + 2;
                triangles[t + 5] = v + 3;

                v += 4;
                t += 6;
            }
        }
    }

    // Contiguous = Smooth edges = Sharing of vertices between quads
    private void MakeContiguousProceduralGrid()
    {
        // Set array size
        vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
        triangles = new int[gridSize * gridSize * 6];

        // Set tracker integers
        int v = 0;
        int t = 0;

        // Set vertex offset
        float vertexOffset = cellSize * 0.5f;

        // Create vertex grid
        for (int x = 0; x <= gridSize; x++)
        {
            for (int z = 0; z <= gridSize; z++)
            {
                vertices[v] = new Vector3((x * cellSize) - vertexOffset, UnityEngine.Random.Range(0f, 1.5f), (z * cellSize) - vertexOffset);
                v++;
            }
        }

        // Reset vertex tracker
        v = 0;

        // Setting each cell's triangles
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                triangles[t] = v;
                triangles[t + 1] = triangles[t + 4] = v + 1;
                triangles[t + 2] = triangles[t + 3] = v + (gridSize + 1);
                triangles[t + 5] = v + (gridSize + 1) + 1;
                v++;
                t += 6;
            }
            v++;
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
