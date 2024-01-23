
using System.Collections;
using UnityEngine;

public class FadingObjectChecker : MonoBehaviour
{
    private RaycastHit[] _hits = new RaycastHit[5];
    private LayerMask _fadeObjectsLayer;
    private Transform _playerTransform;
    
    private YieldInstruction waitForUpdate = new WaitForEndOfFrame();

    private void Awake() => _playerTransform = CharacterPlayerController.Instance.transform;

    private void Start() => StartCoroutine(CheckForObjects());

    private IEnumerator CheckForObjects()
    {
        while (true)
        {
            int hits = Physics.RaycastNonAlloc(
                transform.position, 
                (_playerTransform.transform.position - transform.position),
                _hits, 
                Mathf.Infinity,
                _fadeObjectsLayer);
            yield return waitForUpdate;
        }
    }
}