using System;
using System.Collections.Generic;
using gameplay.ai;
using gameplay.ai.states;
using gameplay.enums;
using UnityEngine;

namespace gameplay
{
    /// <summary>
    /// Autonomous NPC that collects and sorts falling boxes
    /// </summary>
    public class Collector : MonoBehaviour
    {
        [SerializeField] Transform HeldPosition;
        [SerializeField] float moveSpeed = 2f;
        Rigidbody2D character;
        CollectorFSM collectorFSM;
        BoxSpawner boxSpawner;
        GameManager gameManager;
        public CollectorFSM FSM => collectorFSM;
        public Box carriedBox = null;
        void Awake()
        {
            character = GetComponent<Rigidbody2D>();
            character.bodyType = RigidbodyType2D.Kinematic;
            boxSpawner = Finder.Find<BoxSpawner>();
            gameManager = Finder.Find<GameManager>();
            //create the fsm, initializing the different states.
            collectorFSM = new CollectorFSM(new Dictionary<Type, CollectorState>
            {
                {typeof(Idle), new Idle(this)},
                {typeof(SeekingBox), new SeekingBox(this)},
                {typeof(CarryingBox), new CarryingBox(this)},
            });
        }

        /// <summary>
        /// Do updates based on what state our fsm is in
        /// </summary>
        void FixedUpdate()
        {
            collectorFSM.FixedUpdate();
        }

        /// <summary>
        /// handle collision based on which state our fsm is in
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            collectorFSM.OnTriggerEnter2D(other);
        }

        /// <summary>
        /// checks if we already have a box.
        /// </summary>
        /// <returns></returns>
        public bool IsCarryingBox()
        {
            return carriedBox != null;
        }

        /// <summary>
        /// Find the box closest to this position
        /// </summary>
        /// <returns></returns>
        public Box GetNearestBox()
        {
            return boxSpawner.FindNearestBox(transform.position);
        }

        /// <summary>
        /// Get the drop zone for the carried box
        /// </summary>
        /// <returns>The drop zone for the carried boxes type</returns>
        public DropZone GetDropZoneForBox()
        {
            return gameManager.GetDropZone(carriedBox.BoxType);
        }

        /// <summary>
        /// Hand the box off to the drop zone so it can stack them.
        /// </summary>
        /// <param name="dropZone"></param>
        public void DropBox(DropZone dropZone)
        {
            // Debug.Log("Dropping Box at: " + dropZone.name);
            dropZone.DropBox(carriedBox);
            carriedBox = null;
        }

        /// <summary>
        /// Picks up a box on the ground, removes it from the spawner and attaches
        /// it to the agent
        /// </summary>
        /// <param name="box">box to carry</param>
        public void PickUpBox(Box box)
        {
            boxSpawner.DeregisterBox(box);
            carriedBox = box;
            carriedBox.DisablePhysics();
            carriedBox.gameObject.layer = LayerMask.NameToLayer("agent");
            carriedBox.transform.parent = HeldPosition;
            carriedBox.transform.localPosition = Vector3.zero;
        }

        //if the box spawner spawned a box then we can target that box.
        public bool CanTargetBox()
        {
            return boxSpawner.HasBoxes();
        }
        /// <summary>
        /// Start fsm
        /// </summary>
        public void StartCollecting()
        {
            // Debug.Log("StartCollecting");
            collectorFSM.ChangeState<Idle>();
        }

        
        /// <summary>
        /// Used by states to direct the agent towards goals
        /// </summary>
        /// <param name="direction"></param>
        public void Move(Vector2 direction)
        {
            var delta = direction * moveSpeed * Time.fixedDeltaTime;
            var pos = character.position + delta;
            character.MovePosition(pos);
        }
    }
}