namespace ProjectSims.Scripts.StateMachine
{
    public interface IState<T>
    {
        void OnStateEnter(T t);
        void OnStateUpdate(T t);
        void OnStateExit(T t);
    }
}