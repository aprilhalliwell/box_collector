using UnityEngine;

namespace gameplay.ai
{
    public abstract class CollectorState
    {
        public virtual void Enter () { }
        public virtual void Exit () { }
        public virtual void Update (){}
        public virtual void OnTriggerEnter2D(Collider2D other){}
    }
}