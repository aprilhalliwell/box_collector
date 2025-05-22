using UnityEngine;

namespace gameplay.ai.states
{
    /// <summary>
    /// Used to find a box and navigate towards it and pick it up.
    /// </summary>
    public class SeekingBox: CollectorState
    {
        private Collector agent;
        private Box targetedBox;
        private float pickupThreshold = 1f;

        public SeekingBox(Collector agent)
        {
            this.agent = agent;
        }

        override public void Enter()
        {
            //find our nearest box.
            targetedBox = agent.GetNearestBox();
        }  

        /// <summary>
        /// We want to move towards a box until our collector picks up the box
        /// </summary>
        public override void Update()
        {
            //if we failed to find a box go back to idle
            if (targetedBox == null)
            {
                agent.FSM.ChangeState<Idle>();
                return;
            }
            if (agent.IsCarryingBox())
            {
                agent.FSM.ChangeState<CarryingBox>();
                return;
            }
            var direction = (targetedBox.transform.position - agent.transform.position).normalized;
            agent.Move(direction);
        }
        public override void OnTriggerEnter2D(Collider2D other)
        {
            var parentObject = other.transform.parent.gameObject;
            if (parentObject == targetedBox.gameObject)
            {
                agent.PickUpBox(targetedBox);
                agent.FSM.ChangeState<CarryingBox>();
            }
        }
    }
}