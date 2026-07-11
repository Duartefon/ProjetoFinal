using ScriptableObjects;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Gravity;

public class TransitionEffectManager : MonoBehaviour
{
    [SerializeField] private Animator _effectAnimator;

    [SerializeField] private string _effectTrigger = "playGlitch";

    [SerializeField] private float _effectTime = 2.1f;

    [SerializeField] private GameObject _locomotion;
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

    public void TransitionPlayerTo(Transform player, PlayerTransferData destinyData)
    {
        var _characterController = player.GetComponent<CharacterController>();

        _locomotion.GetComponentInChildren<GravityProvider>().ResetFallForce();

        player.transform.position = destinyData.position;
        player.transform.eulerAngles = destinyData.rotation;
        player.transform.localScale = destinyData.scale;

        _characterController.stepOffset = destinyData.stepOffset;

        _locomotion.GetComponentInChildren<GravityProvider>().ResetFallForce();
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