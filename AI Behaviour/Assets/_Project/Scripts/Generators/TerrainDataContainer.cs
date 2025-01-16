using System.IO;
using UnityEngine;

using TerrainData = Terrain_Data.TerrainData;

[CreateAssetMenu(fileName = "NewTerrainDataContainer", menuName = "TerrainDataContainer")]
public class TerrainDataContainer : ScriptableObject
{
    [SerializeField]
    public TerrainData TerrainData;

    public string Path;

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        string path = Path;

        string output = "";

        if (File.Exists(path))
        {
            TextReader textReader = File.OpenText(path);
            output = textReader.ReadToEnd();
        }

        if (!string.IsNullOrEmpty(output))
        {
            TerrainData = JsonUtility.FromJson<TerrainData>(output);
            Debug.Log(TerrainData.MapSize);
        }
    }

}
