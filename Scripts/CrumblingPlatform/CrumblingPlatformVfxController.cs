using Lean.Pool;
using UnityEngine;

public class CrumblingPlatformVfxController : MonoBehaviour
{
    [SerializeField] private GameObject _crumbleSmokeMedium;
    [SerializeField] private GameObject _crumbleSmokeBig;

    private CrumblingPlatform _crumblingPlatform;

    private void Awake() => _crumblingPlatform = GetComponent<CrumblingPlatform>();

    private void OnEnable()
    {
        _crumblingPlatform.OnGetDamaged += CrumblingPlatform_OnGetDamaged;
        _crumblingPlatform.OnCrumble += CrumblingPlatform_OnCrumble;
    }

    private void OnDisable()
    {
        _crumblingPlatform.OnGetDamaged -= CrumblingPlatform_OnGetDamaged;
        _crumblingPlatform.OnCrumble -= CrumblingPlatform_OnCrumble;
    }

    private void CrumblingPlatform_OnGetDamaged() =>
        LeanPool.Spawn(_crumbleSmokeMedium, transform.position, _crumbleSmokeMedium.transform.rotation);

    private void CrumblingPlatform_OnCrumble() =>
        LeanPool.Spawn(_crumbleSmokeBig, transform.position, _crumbleSmokeBig.transform.rotation);
}