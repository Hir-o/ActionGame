using UnityEngine;

public class BaseAchievement : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;

    #region Properties

    public string Name => _name;
    public string Description => _description;

    #endregion
}