using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class OneWorldGenerator : MonoBehaviour
{
    [SerializeField]
    private MeshFilter _meshFilter;
    [SerializeField]
    private MeshRenderer _meshRenderer;

    [Space]

    [SerializeField]
    private string _meshPath = "Assets/";
    [SerializeField]
    private string _meshName;
    [SerializeField]
    private Material _mainMaterial;
    [SerializeField, Range(2, 255)]
    private int _resolution;
    [SerializeField, Min(1)]
    private float _size = 4.0f;
    [SerializeField]
    private Texture2D _heightMap;
    [SerializeField]
    private float _maxHeight;
    
    private Mesh _mesh;

#if UNITY_EDITOR
    [ContextMenu("Generate World Mesh")]
    private void GenerateWorldMesh()
    {
        _mesh = new()
        {
            name = _meshName
        };

        _meshFilter.sharedMesh = _mesh;
        _meshRenderer.sharedMaterial = _mainMaterial;

        Vector3[] verts = new Vector3[_resolution * _resolution];
        int[] tris = new int[2 * 3 * (_resolution - 1) * (_resolution - 1)];
        Vector2[] uvs = new Vector2[_resolution * _resolution];

        //Generate Mesh
        int triIndex = 0;
        Vector3 noisePos;

        for (int y = 0, i = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++, i++)
            {
                Vector2 percentage = new Vector2(x,y) / (_resolution -1);
                //percentage -= Vector2.one * 0.5f;
                
                Vector3 planePos = (Vector3.right * percentage.x + Vector3.forward * percentage.y) * _size;

                Color color = _heightMap.GetPixelBilinear((planePos.x / _size), (planePos.z / _size));

                noisePos = new Vector3(planePos.x, planePos.y + (_maxHeight * color.grayscale), planePos.z);
                verts[i] = noisePos;
                uvs[i] = new Vector2(x, y);

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
        _mesh.uv = uvs;
        _mesh.RecalculateNormals();

    }

    [ContextMenu("Save World Mesh")]
    private void SaveWorldMesh()
    {
        string meshPath = _meshPath;
        meshPath += _meshName;
        meshPath += ".asset";

        AssetDatabase.CreateAsset(_mesh, meshPath);
    }
#endif

}