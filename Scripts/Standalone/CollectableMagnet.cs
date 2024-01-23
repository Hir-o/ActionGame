
using UnityEngine;
using LeonFollowPlayer = Leon.FollowPlayer;

public class CollectableMagnet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out LeonFollowPlayer followPlayer))
            followPlayer.Follow(transform);
    }
}