using ScriptableObjects;
using UnityEngine;

namespace Gameplay
{
    public class FallDetection : MonoBehaviour
    {
        [SerializeField]
        private CharacterController _playerCC;

        [SerializeField] private float fallSpeedThreshold;
        [SerializeField] private PlayerTransferData respawnPosition;
    
 
        // Update is called once per frame
        void FixedUpdate()
        {
            if(_playerCC.velocity.y >= fallSpeedThreshold )
                _playerCC.SendMessage("OnPlayerDeath", respawnPosition );
            
                
        }
    }
}
