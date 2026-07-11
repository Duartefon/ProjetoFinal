using ScriptableObjects;
using UnityEngine;

public class TransitionEffectManager : MonoBehaviour
{
    [SerializeField] private Animator _effectAnimator;

    [SerializeField] private string _effectTrigger = "playGlitch";

    [SerializeField] private float _effectTime = 2.1f;

    
    public float effectTime => _effectTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void PlayEffect()
    {
        if (!_effectAnimator.enabled) _effectAnimator.enabled = true;
        _effectAnimator.SetTrigger(_effectTrigger);
    }
    
    public void PlayEffect(string effectName)
    {
        _effectAnimator.SetTrigger(effectName);
    }
    
    public void TransitionPlayerTo(Transform player, PlayerTransferData destinyData )
    {
        player.transform.position = destinyData.position;
        player.transform.eulerAngles = destinyData.rotation;
        player.transform.localScale = destinyData.scale;

        player.GetComponent<CharacterController>().stepOffset = destinyData.stepOffset;
    }

    public void TransitionPlayerToPositionRotation(Transform player, PlayerTransferData destinyData)
    {
        player.transform.position = destinyData.position;
        player.transform.eulerAngles = destinyData.rotation;
     
    }
    public void SetAnimator(bool active)
    {
        _effectAnimator.enabled = active;
    }
}
