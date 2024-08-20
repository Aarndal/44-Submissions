using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRow : MonoBehaviour
{
    [SerializeField, Min(0)]
    private int _fakePlatformCount;

    private IAmToggleable[] _platforms;

    private void Awake()
    {
        _platforms = GetComponentsInChildren<IAmToggleable>();

        foreach (var platform in _platforms)
            platform.SetToggleState(false);
    }

    private void OnEnable()
    {
        StartCoroutine(ToggleFakePlatforms());
    }

    private IEnumerator ToggleFakePlatforms()
    {
        if (_fakePlatformCount >= _platforms.Length)
            _fakePlatformCount = _platforms.Length - 1;

        yield return null;

        if (_fakePlatformCount > 0)
        {

            //List<IAmToggleable> realPlatforms = new();

            //foreach (var platform in _platforms)
            //{
            //    if (platform is FakePlatform)
            //        realPlatforms.Add(platform);
            //}

            int rnd;

            for (int i = 0; i < _fakePlatformCount; i++)
            {
                do
                {
                    //rnd = Random.Range(0, realPlatforms.Count);
                    rnd = Random.Range(0, _platforms.Length);

                } while (_platforms[rnd].IsActive);

                _platforms[rnd].SetToggleState(true);
                //realPlatforms.RemoveAt(rnd);
            }
        }
    }
}
