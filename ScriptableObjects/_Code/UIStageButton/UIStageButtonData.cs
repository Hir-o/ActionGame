using UnityEngine;

[CreateAssetMenu(fileName = "UIStageButton", menuName = "UIScriptableObject/UIStageButtonData", order = 1)]
public class UIStageButtonData : ScriptableObject
{
    [SerializeField] private int _index;
    [SerializeField] private string _stageName;
    [SerializeField] private Sprite _stageSprite;

    public int Index => _index;
    public string StageName => _stageName;
    public Sprite StageSprite => _stageSprite;
}
