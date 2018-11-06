using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class ProceduralGridMesh : MonoBehaviour {

    public int xSize = 20;
    public int zSize = 20;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    void Start () {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateMeshShape();
        UpdateMesh();

        GetComponent<MeshCollider>().sharedMesh = mesh;
	}
	
	void CreateMeshShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int verts = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = verts + 0;
                triangles[tris + 1] = verts + xSize + 1;
                triangles[tris + 2] = verts + 1;
                triangles[tris + 3] = verts + xSize + 1;
                triangles[tris + 4] = verts + xSize + 2;
                triangles[tris + 5] = verts + 1;

                verts++;
                tris += 6;
            }
            verts++;
        }
    }

    void UpdateMesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
