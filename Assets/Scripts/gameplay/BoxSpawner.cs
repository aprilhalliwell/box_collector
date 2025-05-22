using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using gameplay.enums;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace gameplay
{
    /// <summary>
    /// Simple spawner that spawns and drops boxes based on a random time interval
    /// </summary>
    public class BoxSpawner : MonoBehaviour
    {
        SimplePool simplePool;
        [SerializeField] List<Box> Boxes;
        //whether we should spawn boxes
        [SerializeField] bool shouldSpawn = true;
        //range of the spawn time(aka delay) to handle
        [SerializeField] Vector2 spawnTime = new Vector2(2,5);
        private Coroutine spawnCoroutine;
        void Awake()
        {
            Boxes = new List<Box>();
            //grab our pool
            simplePool = Finder.Find<SimplePool>();
        }

        IEnumerator Spawner()
        {
            while (shouldSpawn)
            {
                var time = Random.Range(spawnTime.x, spawnTime.y);
                yield return new WaitForSeconds(time);
                var box = simplePool.GetObject<Box>();
                if(box != null)
                {
                    var boxType = Random.Range(0f, 1f) > .5 ? BoxType.Blue : BoxType.Red;
                    box.Initialize(transform, boxType, Vector3.zero);
                    Boxes.Add(box);
                }
                else
                {
                    // Debug.Log("All Boxes have been spawned, waiting for released objects");
                }
            }
        }

        public void DeregisterBox(Box box)
        {
            Boxes.Remove(box);
        }

        /// <summary>
        /// Stops the spawner
        /// </summary>
        public void Pause()
        {
            shouldSpawn = false;
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }

        /// <summary>
        /// Starts the spawner
        /// </summary>
        public void Play()
        {
            shouldSpawn = true;
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(Spawner());
            }
        }


        /// <summary>
        /// Checks if we currently have any spawned boxes
        /// </summary>
        /// <returns>true if we have spawned a box, false otherwise</returns>
        public bool HasBoxes()
        {
            return Boxes.Count > 0;
        }

        /// <summary>
        /// Takes the position of an entity and finds the nearest box to that postion
        /// </summary>
        /// <param name="position">entities position</param>
        /// <returns>Nearest box if available, null otherwise </returns>
        public Box FindNearestBox(Vector2 position)
        {
            Box nearestBox = null;
            if (!HasBoxes()) return nearestBox;
            var minDist = float.MaxValue;
            foreach (var box in Boxes)
            {
                var dist = Vector2.Distance(position, box.transform.position);
                //if we have a new min update our check
                if (dist < minDist)
                {
                    nearestBox = box;
                    minDist = dist;
                }
            }
            return nearestBox;
            //alternative using linq
            // var nearestBox = Boxes
            //     .OrderBy(box => Vector2.Distance(position, box.transform.position))
            //     .FirstOrDefault();
            //we could order by the distance and then just take the first item which should be the closest
            //but i think the foreach is more efficient (though we arent doing this per frame so its not as important)
            //and the looping version might be easier to reason about
        }
    }
}