
using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStageButton : MonoBehaviour
{
    [BoxGroup("UI Stage Button Data"), SerializeField]
    private UIStageButtonData _stageData;

    [SerializeField] private TextMeshProUGUI _stageNameText;
    [SerializeField] private Image _stageImage;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private GameObject _newBanner;

    [BoxGroup("CanvasGroup"), SerializeField]
    private float _disabledAlpha = 0.5f;

    private StageSelectUITweener _stageSelectUITweener;
    private Button _button;
    private CanvasGroup _canvasGroup;

    private int _isNewStage;
    private bool _isLocked;

    #region Properties

    public int Index => _stageData.Index;
    public StageSelectUITweener StageSelectUITweener => _stageSelectUITweener;

    #endregion

    private void Awake()
    {
        _stageSelectUITweener = GetComponent<StageSelectUITweener>();
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickStageButton);

        _stageNameText.text = _stageData.StageName;
        _stageImage.sprite = _stageData.StageSprite;
        LoadData();
    }

    private void Start()
    {
        if (_isLocked)
            _newBanner.SetActive(false);
        else
            _newBanner.SetActive(_isNewStage == 1);
    }

    private void OnClickStageButton()
    {
        UIStageSelect.Instance.HandleStageSelectionButtonClick(_stageData.Index);
        _isNewStage = 0;
        _newBanner.SetActive(_isNewStage == 1);
    }

    public void UpdateActiveState(bool enable)
    {
        _canvasGroup.alpha = enable ? 1f : _disabledAlpha;
        _canvasGroup.blocksRaycasts = enable;
        _button.interactable = enable;
        _isLocked = !enable;
        _lockedImage.gameObject.SetActive(_isLocked);
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsConstants.NewStage + _stageData.Index))
            _isNewStage = PlayerPrefs.GetInt(PlayerPrefsConstants.NewStage + _stageData.Index);
        else
        {
            _isNewStage = 1;
            PlayerPrefs.SetInt(PlayerPrefsConstants.NewStage + _stageData.Index, 0);
            PlayerPrefs.Save();
        }
    }
}