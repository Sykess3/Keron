using UnityEngine;

namespace CodeBase.CustomGizmos
{
    public class TriggerGizmo : MonoBehaviour
    {
        public Vector3 Size;

        private void Start() => 
            DestroyImmediate(gameObject);

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color32(30, 200, 30, 130);

            Gizmos.DrawCube(transform.position, Size);
        }
    }
}