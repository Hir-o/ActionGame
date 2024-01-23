using System.Collections;
using UnityEngine;

public interface IBossDashAttack
{
    public IEnumerator DashAttack(Transform[] waypointArray);
}
