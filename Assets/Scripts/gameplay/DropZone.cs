    using System.Collections;
    using System.Collections.Generic;
    using gameplay.enums;
using UnityEngine;

namespace gameplay
{
    /// <summary>
    /// DropZone is used by the agent to place stack boxen
    /// </summary>
    public class DropZone : MonoBehaviour
    {
        [SerializeField] Vector2 despawnRange = new Vector2(1f, 5f);
        [SerializeField] BoxType boxType;
        Queue<Box> leftStack = new Queue<Box>();
        Queue<Box> rightStack = new Queue<Box>();
        [SerializeField] private int maxStackSize = 10;
        [SerializeField] private float horizontalOffset = 0.25f; // space between columns
        private float verticalOffset = .75f; //let the boxes fall from height
        public BoxType BoxType  => boxType;
        private bool isLeft = true;
        
        /// <summary>
        /// Drops a box into the zone.
        /// Handles reparenting of the box and positioning for stacking
        /// </summary>
        /// <param name="box">Box to drop off</param>
        public void DropBox(Box box)
        {
            var stack = isLeft ? leftStack : rightStack;
            var xPos = isLeft ? horizontalOffset : -horizontalOffset;
            //swap direction for next dropoff
            isLeft = !isLeft;
            // check if we need to dequeue a box
            if (stack.Count >= maxStackSize)
            {
                StartCoroutine(DelayedCollapse(stack.Dequeue()));
            }
            stack.Enqueue(box);
            box.transform.SetParent(transform);
            var pos = new Vector3(xPos, verticalOffset);
            box.transform.localPosition = pos;
            box.gameObject.layer = LayerMask.NameToLayer("box");
            box.EnablePhysics();
        }

        IEnumerator DelayedCollapse(Box oldBox)
        {
            yield return new WaitForSeconds(Random.Range(despawnRange.x, despawnRange.y));
            Finder.Find<SimplePool>().ReleaseObject(oldBox.gameObject);
        }
    }
}