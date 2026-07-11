using UnityEngine;
using UnityEngine.VFX; // <-- for VisualEffect

[System.Serializable] // <-- REQUIRED so it serializes/shows in the inspector
public class TransferVFXSettings
{
    [Header("Transfer Progress")]
    [Tooltip("Seconds of aiming to fully charge. Bigger = slower.")]
    [SerializeField, Min(0.01f)]
    private float _secondsToFill = 1f; // renamed from _progressFillSpeed

    [Header("Arc VFX")] [SerializeField] private VisualEffect _arc;

    [Header("Arc Settings  (x = rest / progress 0,  y = full / progress 1)")] [SerializeField]
    private Vector2 _thickness = new Vector2(0.01f, 0.05f);

    [SerializeField] private Vector2 _noisePower = new Vector2(0.15f, 0.5f);
    [SerializeField] private Vector2 _noiseOctaves = new Vector2(3f, 6f);
    [SerializeField] private Vector2 _roughness = new Vector2(0.75f, 1.5f);
    [SerializeField] private Vector2 _noiseLacunarity = new Vector2(1.7f, 2.5f);
    [Tooltip("Base arc color. Alpha is driven by progress (0 → 1) and is ignored here.")]
    [SerializeField, ColorUsage(true, true)] private Color _color = Color.white;
    private float _progress;

    [Header("Blip Material")]
    [Tooltip("Renderer using BlipNMat. Its alpha-clip threshold is driven inversely by progress.")]
    [SerializeField] private Renderer _blipRenderer;

    private Material _blipMaterial;
    private static readonly int CutoffID = Shader.PropertyToID("_Cutoff");
    
    
    private static readonly int ColorID = Shader.PropertyToID("Color");
    private static readonly int ThicknessID = Shader.PropertyToID("Thickness");
    private static readonly int NoisePowerID = Shader.PropertyToID("NoisePower");
    private static readonly int NoiseOctavesID = Shader.PropertyToID("NoiseOctaves");
    private static readonly int RoughnessID = Shader.PropertyToID("Roughness");
    private static readonly int NoiseLacunarityID = Shader.PropertyToID("NoiseLacunarity");
    public float Progress => _progress;
    public bool IsFull => _progress >= 1f;

    // Called every frame from TransferBeam.Update
    
    public void Initialize()
    {
        if (_blipRenderer != null)
            _blipMaterial = _blipRenderer.material;  // runtime instance — won't dirty the asset
    }
    
    
    public void Tick(bool aiming, float deltaTime)
    {
        _progress = aiming
            ? Mathf.Clamp01(_progress + deltaTime / _secondsToFill) // divide, not multiply
            : 0f;

        Apply(_progress);
        
       
        
    }

    private void Apply(float t)
    {
        if (_arc == null) return;

        // RGB (and HDR intensity) stay as authored; alpha ramps 0 → 1 with progress.
        Color c = _color;
        c.a = t;
        _arc.SetVector4(ColorID, c);   // Color = Vector4 in VFX Graph, not a float

        _arc.SetFloat(ThicknessID,       Mathf.Lerp(_thickness.x,       _thickness.y,       t));
        _arc.SetFloat(NoisePowerID,      Mathf.Lerp(_noisePower.x,      _noisePower.y,      t));
        _arc.SetFloat(NoiseOctavesID,    Mathf.Lerp(_noiseOctaves.x,    _noiseOctaves.y,    t));
        _arc.SetFloat(RoughnessID,       Mathf.Lerp(_roughness.x,       _roughness.y,       t));
        _arc.SetFloat(NoiseLacunarityID, Mathf.Lerp(_noiseLacunarity.x, _noiseLacunarity.y, t));
        
        if (_blipMaterial != null)
            _blipMaterial.SetFloat(CutoffID, Mathf.Lerp(1.01f, 0f, t));
    }

    public void DisableVFX(bool value)
    {
        _arc.enabled = value;
    }
}