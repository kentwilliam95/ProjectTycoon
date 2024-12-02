using ProjectSims.Scripts.Place;
using ProjectSims.Scripts.StateMachine;

namespace ProjectSims.Scripts.Entities
{
    public class Work
    {
        protected StateMachine.StateMachine<Work> _stateMachine;
        public Entity Entity { get; private set; }

        public PlaceSO Place { get; private set; }

        public virtual void Initialize(Entity entity, PlaceSO place)
        {
            Entity = entity;
            Place = place;
            _stateMachine = new StateMachine<Work>();
            _stateMachine.SetOwner(this);
        }

        public void Update()
        {
            _stateMachine.Update();
        }

        public void ChangeState(IState<Work> state)
        {
            _stateMachine.ChangeState(state);
        }
    }
}