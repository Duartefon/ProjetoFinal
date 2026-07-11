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
        var cc = player.GetComponent<CharacterController>();
        var gravity = _locomotion.GetComponentInChildren<GravityProvider>();

        // 1. Take the controller offline BEFORE moving/scaling,
        //    so PhysX doesn't try to resolve a giant overlap mid-teleport.
        cc.enabled = false;

        player.position = destinyData.position;
        player.eulerAngles = destinyData.rotation;
        player.localScale = destinyData.scale;   // keep this uniform!

        cc.stepOffset = destinyData.stepOffset;

        // 2. Re-push the shape params so the native controller is rebuilt
        //    with the new lossyScale.
        cc.radius = cc.radius;
        cc.height = cc.height;
        cc.center = cc.center;

        // 3. Flush the transform change into the physics scene.
        Physics.SyncTransforms();

        cc.enabled = true;

        gravity.ResetFallForce();
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