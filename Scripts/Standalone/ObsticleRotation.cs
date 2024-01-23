using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleRotation : MonoBehaviour
{
    public float rotatioSpeed= 1;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(360 * rotatioSpeed * Time.deltaTime, 0, 0);
    }
}
