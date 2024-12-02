namespace ProjectSims.Scripts.StateMachine
{
    public struct StateMachine<T>
    {
        private T _owner;
        private IState<T> _state;
        public IState<T> CurrentState => _state;

        public void SetOwner(T owner)
        {
            _owner = owner;
        }

        public void ChangeState(IState<T> state)
        {
            if(state == null)
                return;
        
            if(_state != null)
                _state.OnStateExit(_owner);
        
            _state = state;
            _state.OnStateEnter(_owner);
        }

        public void Update()
        {
            if(_state == null)
                return;
        
            _state.OnStateUpdate(_owner);
        }
    }
}