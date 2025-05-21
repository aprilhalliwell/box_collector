using System.Collections;
using gameplay.enums;
using UnityEngine;

namespace gameplay
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
    public class Box : MonoBehaviour
    {
        public BoxType BoxType;
        SpriteRenderer spriteRenderer;
        Rigidbody2D rigidbody2D;
        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Transform parent, BoxType boxType, Vector3 position)
        {
            transform.SetParent(parent);
            transform.localPosition = position;
            BoxType = boxType;
            switch (BoxType)
            {
                case BoxType.Red:
                    spriteRenderer.color = Color.red;
                    break;
                case BoxType.Blue:
                    spriteRenderer.color = Color.blue;
                    break;
            }
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Used to disable physics interactions when the box is being held
        /// </summary>
        public void DisablePhysics()
        {
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }

        /// <summary>
        /// Used to enable physics when the box has been dropped off.
        /// </summary>
        public void EnablePhysics()
        {
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}