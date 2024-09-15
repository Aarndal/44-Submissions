using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class PlacementGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private int _density;

    [Space]

    [SerializeField]
    private float _minHeight, _maxHeight;
    [SerializeField]
    private Vector2 _xRange, _zRange;
    [SerializeField]
    private float _verticalOffset;

    [SerializeField, Range(0, 1)]
    private float _rotationTowardsNormal;
    [SerializeField]
    private Vector2 _rotationRange;
    [SerializeField]
    private Vector3 _minScale, _maxScale;

#if UNITY_EDITOR
    [ContextMenu("Generate Prefabs")]
    public void Generate()
    {
        Clear();

        float sampleX, sampleY;
        Vector3 rayStart;

        for (int i = 0; i < _density; i++)
        {
            sampleX = Random.Range(_xRange.x, _xRange.y);
            sampleY = Random.Range(_zRange.x, _zRange.y);
            rayStart = new Vector3(sampleX, _maxHeight, sampleY);

            if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                continue;

            if (hit.point.y < _minHeight)
                continue;

            GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(_prefab, transform);
            instantiatedPrefab.transform.position = hit.point + Vector3.down * _verticalOffset;
            instantiatedPrefab.transform.Rotate(Vector3.up, Random.Range(_rotationRange.x, _rotationRange.y), Space.Self);
            instantiatedPrefab.transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.FromToRotation(instantiatedPrefab.transform.up, hit.normal), _rotationTowardsNormal);

            instantiatedPrefab.transform.localScale = new Vector3(
                Random.Range(_minScale.x, _maxScale.x),
                Random.Range(_minScale.y, _maxScale.y),
                Random.Range(_minScale.z, _maxScale.z)
                );
        }
    }

    [ContextMenu("Clear Prefabs")]
    public void Clear()
    {
        while(transform.childCount !=0)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }
#endif

}