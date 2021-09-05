using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private float _rotationAngleX;
        [SerializeField] private float _distance;
        [SerializeField] private float _offsetY;
        
        [Header("Readonly")]
        [SerializeField] private Transform _target;

        private void LateUpdate()
        {
            if (_target == null)
                return;

            Quaternion rotation = Rotation();
            transform.rotation = rotation;
            transform.position = Position(rotation);
        }

        public void Follow(GameObject target) =>
            _target = target.transform;
        
        private Quaternion Rotation() => Quaternion.Euler(_rotationAngleX, 0f, 0f);

        private Vector3 Position(Quaternion rotation) => rotation * new Vector3(0, 0, -_distance) + FollowingPoint();


        private Vector3 FollowingPoint()
        {
            Vector3 followingPoint = _target.position;
            followingPoint.y += _offsetY;
            return followingPoint;
        }
    }
}