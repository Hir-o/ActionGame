using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Game Controls", order = 2)]
public class GameControlsData : ScriptableObject
{
    [SerializeField] private KeyCode _keyRun;
    [SerializeField] private KeyCode _keyJump;
    [SerializeField] private KeyCode _keySlide;
    [SerializeField] private KeyCode _keyStop;

    public KeyCode KeyRun => _keyRun;
    public KeyCode KeyJump => _keyJump;
    public KeyCode KeySlide => _keySlide;
    public KeyCode KeyStop => _keyStop;
}
