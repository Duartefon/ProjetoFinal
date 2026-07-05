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
        _effectAnimator.SetTrigger(_effectTrigger);
    }
}
