using UnityEngine;

namespace gameplay.ai.states
{
    /// <summary>
    /// Simple state that transitions to whether we are seeking or carrying a box
    /// based on the agents state.
    /// </summary>
    public class Idle : CollectorState
    {
        private Collector agent;

        public Idle(Collector agent)
        {
            this.agent = agent;
        }

        public override void Enter()
        {
            // Debug.Log("Going to Idle");
        }

        public override void Update()
        {
            if (agent.IsCarryingBox())
            {
                agent.FSM.ChangeState<CarryingBox>();
            }
            else if (agent.CanTargetBox())
            {
                agent.FSM.ChangeState<SeekingBox>();
            }
        }
    }
}