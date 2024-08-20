using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField]
    FakePlatform platform;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
            if (platform.IsActive)
                StartCoroutine(DeactivatePlatform());
    }

    private IEnumerator DeactivatePlatform()
    {
        yield return new WaitForSeconds(platform.DeactivationTime);
        platform.gameObject.SetActive(false);
    }
}
