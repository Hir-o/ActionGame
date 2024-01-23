using System.Collections;
using UnityEngine;

public interface IExecuteWithDelay
{
    public float Delay { get; set; }
    public WaitForSeconds Wait { get; set; }

    IEnumerator ExecuteWithDelay();
}