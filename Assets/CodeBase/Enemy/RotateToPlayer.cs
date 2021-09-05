using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToPlayer : Followable
    {
        private void LateUpdate()
        {
            if (IsInitializedHeroTransform()) 
                RotateTowardsHero();
        }

        private void RotateTowardsHero()
        {
            Vector3 target = PositionForLook(at: Target.position);
            transform.rotation = SmoothedRotation(to: target);
        }

        private Vector3 PositionForLook(Vector3 at)
        {
            Vector3 positionDifference = at - transform.position;
            return new Vector3(positionDifference.x, transform.position.y, positionDifference.z);
        }
        
        private Quaternion SmoothedRotation(Vector3 to) => 
            Quaternion.Lerp(transform.rotation, TargetRotation(to), SpeedFactor());

        private static Quaternion TargetRotation(Vector3 to) => 
            Quaternion.LookRotation(to);

        private float SpeedFactor() => 
            Speed *Time.deltaTime;
        
        private bool IsInitializedHeroTransform() =>
            Target != null;
    }
}