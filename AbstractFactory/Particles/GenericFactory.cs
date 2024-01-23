

using Lean.Pool;
using Leon;
using UnityEngine;

namespace Factory
{
    public abstract class GenericFactory<T> : SceneSingleton<T> where T : MonoBehaviour
    {
        public abstract GameObject GetNewInstance(GameObject gameObject, Vector3 spawnPosition);
    }
}