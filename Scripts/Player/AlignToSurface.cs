

using NaughtyAttributes;
using UnityEngine;

public class AlignToSurface : MonoBehaviour
{
    [BoxGroup("Ray Origins"), SerializeField]
    private Transform _leftRay;

    [BoxGroup("Ray Origins"), SerializeField]
    private Transform _rightRay;

    [BoxGroup("Surface Detection"), SerializeField]
    private LayerMask _layerMask;

    [BoxGroup("Surface Detection"), SerializeField]
    private float _rayLength = 1;

    [BoxGroup("Rotation Parameters"), SerializeField]
    private float _rotationSpeed = 5f;

    [BoxGroup("Rotation Parameters"), SerializeField]
    private AnimationCurve _rotationCurve;

    [BoxGroup("Rotation Parameters"), SerializeField]
    private float _maxRotationAngle = 20f;

    public void SurfaceAlignment()
    {
        Quaternion _leftQuaternion = GetDestinedQuaternion(_leftRay);
        Quaternion _rightQuaternion = GetDestinedQuaternion(_rightRay);
        
        if (_leftQuaternion == _rightQuaternion)
            transform.localRotation =
                Quaternion.Lerp(transform.localRotation, _leftQuaternion, _rotationCurve.Evaluate(_rotationSpeed));
        else
            transform.localRotation =
                Quaternion.Lerp(transform.localRotation, Quaternion.identity, _rotationCurve.Evaluate(_rotationSpeed));
    }

    private Quaternion GetDestinedQuaternion(Transform origin)
    {
        Quaternion finalQuaternion = Quaternion.identity;
        Ray ray = new Ray(origin.position, -transform.up);
        RaycastHit _raycastHit = new RaycastHit();
        if (Physics.Raycast(ray, out _raycastHit, _rayLength, _layerMask))
        {
            finalQuaternion = Quaternion.FromToRotation(Vector3.up, _raycastHit.normal);
            finalQuaternion.z = Mathf.Clamp(finalQuaternion.z, -_maxRotationAngle, _maxRotationAngle);
            finalQuaternion.x = -finalQuaternion.z;
            finalQuaternion.y = 0f;
            finalQuaternion.z = 0f;
        }

        return finalQuaternion;
    }
}