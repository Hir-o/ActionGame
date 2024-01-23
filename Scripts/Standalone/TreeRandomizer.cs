
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TreeRandomizer : MonoBehaviour
{
    [BoxGroup("Meshes"), SerializeField] private List<Mesh> _meshList = new List<Mesh>();

    [BoxGroup("Trees"), SerializeField] private List<MeshFilter> _treeMeshFilterList = new List<MeshFilter>();

    [BoxGroup("Scale"), SerializeField] private float _minScale = 1f;
    [BoxGroup("Scale"), SerializeField] private float _maxScale = 1.3f;

    [BoxGroup("Rotation"), SerializeField] private float _minYRotation;
    [BoxGroup("Rotation"), SerializeField] private float _maxYRotation = 360f;


    private void Awake()
    {
        if (_meshList.Count > 0 && _treeMeshFilterList.Count > 0)
            Randomize();
        else
            Debug.LogError("Conditions for randomizing trees are not met. Remove this gameObject: " + gameObject.name);
    }

    private void Randomize()
    {
        _treeMeshFilterList.ForEach((treeFilter) =>
        {
            int random = Random.Range(0, _meshList.Count);
            float randomScale = Random.Range(_minScale, _maxScale);
            float _randomRotationAngle = Random.Range(_minYRotation, _maxYRotation);
            treeFilter.sharedMesh = _meshList[random];
            treeFilter.transform.localScale = Vector3.one * randomScale;
            treeFilter.transform.localRotation = Quaternion.Euler(treeFilter.transform.localRotation.x,
                _randomRotationAngle, treeFilter.transform.localRotation.z);
        });
    }
}