using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {

    Mesh mesh;

	// Use this for initialization
	void Start () {
        mesh = new Mesh();

        GenerateMesh();

        GetComponent<MeshFilter>().mesh = mesh;
	}

    private void GenerateMesh()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1)
        };

        int[] triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3
        };

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
