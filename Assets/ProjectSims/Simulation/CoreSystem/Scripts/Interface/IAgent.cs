using System.Numerics;
using Vector3 = UnityEngine.Vector3;

namespace ProjectSims.Simulation.CoreSystem.Scripts.Interface
{
    public interface IAgent
    {
        public void MoveTo(Vector3 target);
        public void DisableAgent();
        public void EnableAgent();
    }
}