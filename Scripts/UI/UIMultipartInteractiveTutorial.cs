
using DG.Tweening;
using Leon;
using UnityEngine;

public class UIMultipartInteractiveTutorial : SceneSingleton<UIMultipartInteractiveTutorial>
{
    private UITutorial _uiTutorial;
    [SerializeField] private UITutorialPart[] _uiTutorialPartArray;

    private Tween _disableJumpTween;
    
    private int _tutorialIndex;
    
    #region Properties

    public bool IsTutorialEnabled => _uiTutorial.enabled;
    
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _uiTutorial = GetComponent<UITutorial>();
    }

    public void EnableTutorial()
    {
        if (_tutorialIndex >= _uiTutorialPartArray.Length) return;
        if (_uiTutorialPartArray[_tutorialIndex].EnableJumping)
        {
            MovementController.Instance.CanJump = true;
            if (_uiTutorialPartArray[_tutorialIndex].DisableJumpingAfterEnabling)
            {
                if (_disableJumpTween != null) _disableJumpTween.Kill();
                _disableJumpTween = DOVirtual.Float(1f, 0f, .25f, value => { })
                    .OnComplete(() => MovementController.Instance.CanJump = false);
            }
        }
        if (_uiTutorialPartArray[_tutorialIndex].EnableDoubleJumping) MovementController.Instance.CanDoubleJump = true;
        if (_uiTutorialPartArray[_tutorialIndex].EnableGliding)
        {
            MovementController.Instance.GlideAction.enabled = true;
            MovementController.Instance.GlideAction.IsEnabled = true;
        }
        _uiTutorial.enabled = true;
        _uiTutorialPartArray[_tutorialIndex].TutorialPanelTweener.gameObject.SetActive(true);
        _uiTutorialPartArray[_tutorialIndex].TutorialPanelTweener.enabled = true;
        _tutorialIndex++;
    }
}