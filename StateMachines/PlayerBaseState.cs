public abstract class PlayerBaseState
{
    private PlayerStateMachine _context;
    private PlayerStateFactory _playerStateFactory;

    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    private bool _isRootState = false;

    protected bool IsRootState
    {
        get => _isRootState;
        set => _isRootState = value;
    }

    protected PlayerStateMachine Context => _context;
    protected PlayerStateFactory PlayerStateFactory => _playerStateFactory;
    
    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
    {
        _context = context;
        _playerStateFactory = playerStateFactory;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null) _currentSubState.UpdateStates();
    }

    protected void SwitchState(PlayerBaseState newState)
    {
        ExitState();
        newState.EnterState();
        
        if (_isRootState)
            _context.CurrentState = newState;
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState);
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
