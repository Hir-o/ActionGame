
using UnityEngine;

public class TutorialTrigger : Invisibler, IUngrabbable
{
    private bool _isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_isTriggered) return;
        if (other.gameObject.TryGetComponent(out CharacterPlayerController player))
        {
            if (UIMultipartInteractiveTutorial.Instance != null)
                UIMultipartInteractiveTutorial.Instance.EnableTutorial();
            else
                UIInteractiveTutorial.Instance.EnableTutorial();
            
            TimeScaleManager.Instance.UpdateTimeScale(0f, true);
            _isTriggered = true;
        }
    }
}