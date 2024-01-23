
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Leon;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILevelSelection : SceneSingleton<UILevelSelection>
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [BoxGroup("Tweening"), SerializeField] private float _fadeSpeed = 10f;
    [BoxGroup("Tweening"), SerializeField] private Ease _fadeEase = Ease.OutQuad;

    [SerializeField] private Button _backButton;
    [SerializeField] private LevelButtonsTweener[] _evelButtonsTweenerArray;

    private List<UILevelSelectButton> _levelSelectButtonList = new List<UILevelSelectButton>();

    #region Properties

    public List<UILevelSelectButton> LevelSelectButtonList => _levelSelectButtonList;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _backButton.onClick.AddListener(OnClickBackButton);

        _levelSelectButtonList = GetComponentsInChildren<UILevelSelectButton>(true).ToList();
    }

    public void HandleLevelSelectionButtonClick(int index)
    {
        UIFader.Instance.FadeIn();
        FadeOut(false, index);
    }

    public void OnClickBackButton() => FadeOut();

    public void FadeIn(int selectedStageIndex = 0)
    {
        for (int i = 0; i < _evelButtonsTweenerArray.Length; i++)
        {
            _evelButtonsTweenerArray[i].gameObject.SetActive(false);
            _evelButtonsTweenerArray[i].Reset();
        }

        _evelButtonsTweenerArray[selectedStageIndex].gameObject.SetActive(true);
        _evelButtonsTweenerArray[selectedStageIndex].AnimateButtons();
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase);
    }

    public void FadeOut(bool isBackButtonClicked = true, int sceneIndex = 0)
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0f, _fadeSpeed)
            .SetSpeedBased()
            .SetEase(_fadeEase)
            .OnComplete(() =>
            {
                if (isBackButtonClicked)
                    UIStageSelect.Instance.FadeIn();
                else
                {
                    DOTween.KillAll();
                    SceneManager.LoadScene(sceneIndex);
                }
            });
    }
}