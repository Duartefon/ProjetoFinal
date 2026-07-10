using UnityEngine;
using UnityEngine.VFX;                 // <-- for VisualEffect

[System.Serializable]                  // <-- REQUIRED so it serializes/shows in the inspector
public class TransferVFXSettings
{
    [Header("Transfer Progress")]
    [Tooltip("Seconds of aiming to fully charge. Bigger = slower.")]
    [SerializeField, Min(0.01f)] private float _secondsToFill = 1f;   // renamed from _progressFillSpeed

    [Header("Arc VFX")]
    [SerializeField] private VisualEffect _arc;

    [Header("Arc Settings  (x = rest / progress 0,  y = full / progress 1)")]
    [SerializeField] private Vector2 _thickness       = new Vector2(0.01f, 0.05f);
    [SerializeField] private Vector2 _noisePower      = new Vector2(0.15f, 0.5f);
    [SerializeField] private Vector2 _noiseOctaves    = new Vector2(3f,    6f);
    [SerializeField] private Vector2 _roughness       = new Vector2(0.75f, 1.5f);
    [SerializeField] private Vector2 _noiseLacunarity = new Vector2(1.7f,  2.5f);

    private float _progress;

    // Must match the EXPOSED property names in your VFX Graph blackboard
    // (case-sensitive — VFX names usually have NO leading underscore, unlike shader props)
    private static readonly int ThicknessID       = Shader.PropertyToID("Thickness");
    private static readonly int NoisePowerID       = Shader.PropertyToID("NoisePower");
    private static readonly int NoiseOctavesID      = Shader.PropertyToID("NoiseOctaves");
    private static readonly int RoughnessID         = Shader.PropertyToID("Roughness");
    private static readonly int NoiseLacunarityID   = Shader.PropertyToID("NoiseLacunarity");

    public float Progress => _progress;
    public bool IsFull => _progress >= 1f;

    // Called every frame from TransferBeam.Update
    public void Tick(bool aiming, float deltaTime)
    {
        _progress = aiming
            ? Mathf.Clamp01(_progress + deltaTime / _secondsToFill)   // divide, not multiply
            : 0f;

        Apply(_progress);
    }
    private void Apply(float t)
    {
        if (_arc == null) return;

        _arc.SetFloat(ThicknessID,       Mathf.Lerp(_thickness.x,       _thickness.y,       t));
        _arc.SetFloat(NoisePowerID,      Mathf.Lerp(_noisePower.x,      _noisePower.y,      t));
        _arc.SetFloat(NoiseOctavesID,    Mathf.Lerp(_noiseOctaves.x,    _noiseOctaves.y,    t)); // see note on octaves
        _arc.SetFloat(RoughnessID,       Mathf.Lerp(_roughness.x,       _roughness.y,       t));
        _arc.SetFloat(NoiseLacunarityID, Mathf.Lerp(_noiseLacunarity.x, _noiseLacunarity.y, t));
    }
}