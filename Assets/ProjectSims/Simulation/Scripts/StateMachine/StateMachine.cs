namespace ProjectSims.Simulation.Scripts.StateMachine
{
    public class StateMachine<T>
    {
        private IState<T> _current;
        public IState<T> CurrentState => _current;
        
        private T _owner;

        public StateMachine(T t)
        {
            _owner = t;
        }

        public void ChangeState(IState<T> next)
        {
            if (_current != null)
            {
                _current.OnExit(_owner);
            }

            _current = next;
            _current.OnEnter(_owner);
        }

        public void Update()
        {
            if (_current == null) { return;}
            _current.OnUpdate(_owner);
        }
    }
}