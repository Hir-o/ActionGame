using UnityEngine;

public class SwingingRope : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.TryGetComponent(out CharacterPlayerController controller))
    }

    public void OnHang()
    {
        Debug.Log("On Player Hang on Swinging Rope");
    }
}
