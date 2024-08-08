using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlaneGenerator : MonoBehaviour
{
    private const float NoiseOffSetX = 509;
    private const float NoiseOffSetY = 241;
    
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private Material _mainMaterial;
    [SerializeField, Range(2, 255)]
    private int _resolution;
    [SerializeField, Min(1)]
    private float _size = 4.0f;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _mesh = new();
        _mesh.name = "MyPlaneGenerator";
        _meshFilter.sharedMesh = _mesh;
        _meshRenderer.sharedMaterial = _mainMaterial;
    }

    private void Update()
    {
        GeneratePlane();
    }

    private void GeneratePlane()
    {
        Vector3[] verts = new Vector3[_resolution * _resolution];
        int[] tris = new int[2 * 3 * (_resolution - 1) * (_resolution - 1)];

        //Generate Mesh
        int triIndex = 0;
        for (int y = 0, i = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++, i++)
            {
                Vector2 percentage = new Vector2(x,y) / (_resolution -1);
                percentage -= Vector2.one * 0.5f;

                Vector3 planePos = (Vector3.right * percentage.x + Vector3.forward * percentage.y) * _size;
                verts[i] = planePos;
                //Vector3 noisePos = planePos + Vector3.up * Mathf.PerlinNoise(planePos.x + NoiseOffSetX, planePos.z + NoiseOffSetY);
                //verts[i] = noisePos;

                if (y < _resolution - 1 && x < _resolution - 1)
                {
                    //This vertex has to build a quad (2 triangles)!
                    tris[triIndex + 0] = i;
                    tris[triIndex + 1] = i + _resolution + 1;
                    tris[triIndex + 2] = i + 1;

                    tris[triIndex + 3] = i;
                    tris[triIndex + 4] = i + _resolution;
                    tris[triIndex + 5] = i + _resolution + 1;

                    triIndex += 6;
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = verts;
        _mesh.triangles = tris;
        _mesh.RecalculateNormals();
    }
}
