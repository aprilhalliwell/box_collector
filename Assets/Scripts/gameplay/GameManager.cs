using System.Collections.Generic;
using System.Linq;
using gameplay.enums;
using UnityEngine;
using UnityEngine.UI;
using utils;

namespace gameplay
{
    /// <summary>
    /// Simple manager to orchestrate game start
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] Button startButton;
        [SerializeField] Collector collector;
        List<DropZone> dropZones;
        ClickShield clickShield;
        BoxSpawner boxSpawner;
        
        void Awake()
        {
            dropZones = FindObjectsByType<DropZone>(FindObjectsSortMode.None).ToList();
            clickShield = Finder.Find<ClickShield>();
            boxSpawner = Finder.Find<BoxSpawner>();
        }

        void Start()
        {
            startButton.onClick.AddListener(PlayGame);
        }

        void OnDestroy()
        {
            startButton.onClick.RemoveListener(PlayGame);
        }


        /// <summary>
        /// Finds a drop zone based on the boxes type
        /// </summary>
        /// <param name="boxType"></param>
        /// <returns>Drop zone for the box type</returns>
        public DropZone GetDropZone(BoxType boxType)
        {
            return dropZones.Find(d => d.BoxType == boxType);
        }

        /// <summary>
        /// Called on click
        /// we should hide the click shield and start the game systems.
        /// </summary>
        void PlayGame()
        {
            startButton.gameObject.SetActive(false);
            clickShield.Shield(false);
            boxSpawner.Play();
            collector.StartCollecting();
        }
    }
}