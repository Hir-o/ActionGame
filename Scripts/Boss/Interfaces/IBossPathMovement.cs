using System.Collections;
using UnityEngine;

public interface IBossPathMovement
{
    public IEnumerator MovePath(Transform[] waypointArray);
}
