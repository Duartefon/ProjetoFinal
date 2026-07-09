using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/playerTransforms")]
    public class PlayerTransferData : ScriptableObject
    {
        public Vector3  position;
        public Vector3 rotation;
        public Vector3 scale;
        public float moveSpeed;
        public float stepOffset;
    }
}
