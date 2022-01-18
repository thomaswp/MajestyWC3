using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;
using static War3Api.Blizzard;
using Source;

namespace Source.Behaviors
{
    public abstract class TargetBehavior<T> : Behavior  where T : class
    {
        protected abstract T SelectTarget();

        private T _target;
        public T Target {
            get { return _target; }
            protected set 
            {
                T oldTarget = _target;
                _target = value;
                if (_target != null)
                {
                    OnTargetUpdated();
                } else if (oldTarget != null)
                {
                    OnTargetCleared();
                }
            } 
        }

        private timer timeoutTimer;

        protected virtual int GetTargetTimeout()
        {
            return int.MaxValue;
        }

        public override bool CanStart()
        {
            UpdateTarget();
            return IsTargetStillValid(Target);
        }

        public override void Start()
        {
            UpdateTarget();
        }

        public override void Stop()
        {
            Target = null;
        }

        public override bool Update()
        {
            return IsTargetStillValid(Target);
        }

        protected virtual bool IsTargetStillValid(T target)
        {
            return target != null;
        }

        private void UpdateTarget()
        {
            if (timeoutTimer != null)
            {
                float elapsed = TimerGetElapsed(timeoutTimer);
                if (elapsed >= GetTargetTimeout())
                {
                    //Console.WriteLine("Clearing target due to timeout");
                    Target = null;
                }
            }
            if (IsTargetStillValid(Target)) return;
            if (timeoutTimer != null) DestroyTimer(timeoutTimer);
            timeoutTimer = null;
            // Set to null first to trigger the clear;
            Target = null;
            RefreshTarget();
        }

        protected bool RefreshTarget()
        {
            T newTarget = SelectTarget();
            if (newTarget == null || newTarget == Target) return false;
            Target = newTarget;
            int timeout = GetTargetTimeout();
            if (timeout != int.MaxValue)
            {
                timeoutTimer = CreateTimer();
                StartTimerBJ(timeoutTimer, false, timeout);
            }
            OnTargetUpdated();
            return true;
        }

        protected virtual void OnTargetUpdated()
        {

        }

        protected virtual void OnTargetCleared()
        {

        }
    }
}
