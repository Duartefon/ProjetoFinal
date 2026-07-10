using ScriptableObjects;
using UnityEngine;

namespace Gameplay
{
  class PlayerLife : MonoBehaviour
  {
      [SerializeField] private TransitionEffectManager _transitionEffectManager;
      
      public void OnPlayerDeath(PlayerTransferData playerData)
        {
            _transitionEffectManager.TransitionPlayerTo(this.transform, playerData);
            _transitionEffectManager.PlayEffect();
            
        }
    }
}
