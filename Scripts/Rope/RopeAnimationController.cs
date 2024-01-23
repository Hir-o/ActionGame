using UnityEngine;

public class RopeAnimationController : MonoBehaviour
{
    [SerializeField] private bool _mirrorRopeSwingAnimation;

    private Animator _animator;

    private const string _isMirror = "isMirror";

    private void Awake() => _animator = GetComponentInChildren<Animator>();

    private void Start() => _animator.SetBool(_isMirror, _mirrorRopeSwingAnimation);
}