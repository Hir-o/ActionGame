using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Camera Settings/Camera Profile", order = 1)]
public class CameraSettingsData : ScriptableObject
{
    [Header("FOV"), SerializeField] private float _idleVerticalFOV = 50f;
    [SerializeField] private float _movingVerticalFOX = 60f;

    [Header("Transposer Damping"), SerializeField] private float _xDamping = 1f;
    [SerializeField] private float _yDamping = 1f;
    [SerializeField] private float _zDamping = 1f;

    [Header("Follow Offset"), SerializeField]
    private Vector3 _idleFollowOffset;

    [SerializeField] private Vector3 _movingFollowOffset;

    [Header("Screen Y"), SerializeField] private float _idleScreenY = 0.5f;
    [SerializeField] private float _movingScreenY = 0.45f;

    [Header("Look Ahead"), SerializeField] private float _lookAheadTime = 0.1f;
    [SerializeField] private float _lookAheadSmoothing = 7f;

    [Header("Vertical and Horizontal Dampening"), SerializeField]
    private float _horizontalDampening = 2f;

    [SerializeField] private float _verticalDampening = 3f;

    public float IdleVerticalFOV => _idleVerticalFOV;
    public float MovingVerticalFOV => _movingVerticalFOX;
    public float XDamping => _xDamping;
    public float YDamping => _yDamping;
    public float ZDamping => _zDamping;
    public Vector3 IdleFollowOffset => _idleFollowOffset;
    public Vector3 MovingFollowOffset => _movingFollowOffset;
    public float IdleScreenY => _idleScreenY;
    public float MovingScreenY => _movingScreenY;
    public float LookAheadTime => _lookAheadTime;
    public float LookAheadSmoothing => _lookAheadSmoothing;
    public float HorizontalDampening => _horizontalDampening;
    public float VerticalDampening => _verticalDampening;
}