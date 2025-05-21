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
        [SerializeField] BoxType boxType;
        List<Box> leftStack = new List<Box>();
        List<Box> rightStack = new List<Box>();
        [SerializeField] private float horizontalOffset = 0.25f; // space between columns
        private float verticalOffset = .75f; //let the boxes fall from height
        public BoxType BoxType  => boxType;

        /// <summary>
        /// Drops a box into the zone.
        /// Handles reparenting of the box and positioning for stacking
        /// </summary>
        /// <param name="box">Box to drop off</param>
        public void DropBox(Box box)
        {
            var lOrR = leftStack.Count < rightStack.Count;
            var stack = lOrR ? leftStack : rightStack;
            var xPos = lOrR ? horizontalOffset : -horizontalOffset;
            stack.Add(box);
            box.transform.SetParent(transform);
            var pos = new Vector3(xPos, verticalOffset);
            box.transform.localPosition = pos;
            box.EnablePhysics();
        }
    }
}