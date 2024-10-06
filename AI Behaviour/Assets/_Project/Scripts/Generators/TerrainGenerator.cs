using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;

public partial class TerrainGenerator : MonoBehaviour
{
    //[SerializeField]
    //private string _path = "Assets/";
    [SerializeField]
    private string _name;
    [SerializeField]
    private Material _mainMaterial;
    [SerializeField, Range(2, 255)]
    private int _resolution;
    [SerializeField, Min(1)]
    private float _edgeLength = 4.0f;
    [SerializeField, Min(1)]
    private int _chunksPerRow = 4;
    [SerializeField]
    private Texture2D _heightMap;
    [SerializeField]
    private float _maxHeight;

    public TerrainGenerationChunk _chunk;

#if UNITY_EDITOR
    [ContextMenu("Generate Terrain")]
    private async void GenerateTerrain()
    {
        if(transform.childCount > 0)
        {
            int i = 0;

            GameObject[] allChildren = new GameObject[transform.childCount];

            foreach (Transform child in transform)
            {
                allChildren[i] = child.gameObject;
                i += 1;
            }

            foreach (GameObject child in allChildren)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        float chunkEdgeLength = _edgeLength / (float)(_chunksPerRow);
        TerrainGenerationChunk[] chunks = new TerrainGenerationChunk[_chunksPerRow * _chunksPerRow];
        Task[] createChunk = new Task[_chunksPerRow * _chunksPerRow];

        for (int v = 0, j = 0; v < _chunksPerRow; v++)
        {
            for (int u = 0; u < _chunksPerRow; u++, j++)
            {
                chunks[j] = Instantiate(_chunk, this.transform.position + chunkEdgeLength * u * Vector3.right + chunkEdgeLength * v * Vector3.forward, Quaternion.identity, transform);
                chunks[j].name = string.Format("Chunk {0:D2}", j);
                chunks[j].GetMeshComponents(_mainMaterial);
                createChunk[j] = chunks[j].GenerateChunkMesh(j, u, v, _resolution, _edgeLength, chunkEdgeLength, _maxHeight, _heightMap);
            }
        }

        await Task.WhenAll(createChunk);
    }

    //[ContextMenu("Save Terrain")]
    //private void SaveTerrain()
    //{
    //    string path = _path;
    //    path += _name;
    //    path += ".asset";

    //    AssetDatabase.CreateAsset(this, path);
    //}
#endif
}