using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class TerrainGenerationChunk : MonoBehaviour
{
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    private Material _mainMaterial;

    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] _uvs;

    public void GetMeshComponents(Material mainMaterial)
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _mainMaterial = mainMaterial;
        _meshCollider = GetComponent<MeshCollider>();

        _mesh = new()
        {
            //name = "Chunk " + chunkNumber.ToString()
        };

        _meshFilter.sharedMesh = _mesh;
        _meshRenderer.sharedMaterial = _mainMaterial;

    }

    public async Task GenerateChunkMesh(int chunkNumber, int u, int v, int resolution, float edgeLength, float chunkEdgeLength, float maxHeight, Texture2D _heightMap)
    {
        //_mesh.name = "Chunk " + chunkNumber.ToString();

        //Chunk Members
        _vertices = new Vector3[resolution * resolution];
        _triangles = new int[2 * 3 * (resolution - 1) * (resolution - 1)];
        _uvs = new Vector2[resolution * resolution];

        //Mesh Generation Variables
        int triIndex = 0;
        Vector3 noisePos;

        for (int y = 0, i = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++, i++)
            {
                Vector2 percentage = new Vector2(x, y) / (resolution - 1);

                Vector3 planePos = (Vector3.right * percentage.x + Vector3.forward * percentage.y) * chunkEdgeLength;

                Color color = _heightMap.GetPixelBilinear((planePos.x + u * chunkEdgeLength) / edgeLength, (planePos.z + v * chunkEdgeLength) / edgeLength);

                noisePos = new Vector3(planePos.x, planePos.y + (maxHeight * color.grayscale), planePos.z);
                _vertices[i] = noisePos;
                _uvs[i] = new Vector2(x, y);

                if (y < resolution - 1 && x < resolution - 1)
                {
                    //This vertex has to build a quad (2 triangles)!
                    _triangles[triIndex + 0] = i;
                    _triangles[triIndex + 1] = i + resolution + 1;
                    _triangles[triIndex + 2] = i + 1;

                    _triangles[triIndex + 3] = i;
                    _triangles[triIndex + 4] = i + resolution;
                    _triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6;
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs;
        _mesh.RecalculateNormals();

        _meshCollider.sharedMesh = _mesh;
        _meshCollider.isTrigger = false;
        await Task.Yield();
    }

    public void GenerateChunkMeshSync(int chunkNumber, int u, int v, int resolution, float edgeLength, float chunkEdgeLength, float maxHeight, Texture2D _heightMap)
    {
        //_mesh = new()
        //{
        //    name = "Chunk " + chunkNumber.ToString()
        //};

        //_meshFilter.sharedMesh = _mesh;
        //_meshRenderer.sharedMaterial = _mainMaterial;

        //Chunk Members
        _vertices = new Vector3[resolution * resolution];
        _triangles = new int[2 * 3 * (resolution - 1) * (resolution - 1)];
        _uvs = new Vector2[resolution * resolution];

        //Mesh Generation Variables
        int triIndex = 0;
        Vector3 noisePos;

        for (int y = 0, i = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++, i++)
            {
                Vector2 percentage = new Vector2(x, y) / (resolution - 1);

                Vector3 planePos = (Vector3.right * percentage.x + Vector3.forward * percentage.y) * chunkEdgeLength;

                Color color = _heightMap.GetPixelBilinear((planePos.x + u * chunkEdgeLength) / edgeLength, (planePos.z + v * chunkEdgeLength) / edgeLength); // TODO

                noisePos = new Vector3(planePos.x, planePos.y + (maxHeight * color.grayscale), planePos.z);
                _vertices[i] = noisePos;
                _uvs[i] = new Vector2(x, y);

                if (y < resolution - 1 && x < resolution - 1)
                {
                    //This vertex has to build a quad (2 triangles)!
                    _triangles[triIndex + 0] = i;
                    _triangles[triIndex + 1] = i + resolution + 1;
                    _triangles[triIndex + 2] = i + 1;

                    _triangles[triIndex + 3] = i;
                    _triangles[triIndex + 4] = i + resolution;
                    _triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6;
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs;
        _mesh.RecalculateNormals();

        _meshCollider.sharedMesh = _mesh;
        _meshCollider.isTrigger = false;
    }
}

