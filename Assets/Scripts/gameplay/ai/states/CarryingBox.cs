using UnityEngine;

namespace gameplay.ai.states
{
    /// <summary>
    /// Used to drop a box off at a target zone. 
    /// </summary>
    public class CarryingBox: CollectorState
    {
        private Collector agent;
        private DropZone targetDropZone;
        private float dropThreshold = 1.3f;
        public CarryingBox(Collector agent)
        {
            this.agent = agent;
        }

        public override void Enter()
        {
            //first find our drop zone
            targetDropZone = agent.GetDropZoneForBox();
            // Debug.Log($"Carrying box to : {targetDropZone.BoxType}-{targetDropZone.name} for {agent.carriedBox.BoxType}");
        }
        public override void Update()
        {
            //if we failed to find a zone go back to idle
            //or if we are no longer carrying a box
            if (targetDropZone == null || !agent.IsCarryingBox())
            {
                agent.FSM.ChangeState<Idle>();
                return;
            }
            var direction = (targetDropZone.transform.position - agent.transform.position).normalized;
            agent.Move(direction);
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == targetDropZone.gameObject)
            {
                agent.DropBox(targetDropZone);
                agent.FSM.ChangeState<Idle>();
            }
        }
    }
}