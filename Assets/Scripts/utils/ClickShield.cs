using UnityEngine;
using UnityEngine.UI;

namespace utils
{
    /// <summary>
    /// Util for showing a click shield
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ClickShield : MonoBehaviour
    {
        private Image shield;
        void Awake()
        {
            shield = GetComponent<Image>();
        }

        public void Shield(bool block)
        {
            shield.enabled = block;
        }
    }
}