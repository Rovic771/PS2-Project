using UnityEngine;

/// <summary>
/// Simple controller for godray effects. Attach to a godray object to control its appearance at runtime.
/// </summary>
public class GodrayController : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] [ColorUsage(false, true)] private Color rayColor = new Color(1f, 0.95f, 0.8f, 1f);
    [SerializeField] [Range(0f, 2f)] private float intensity = 1f;
    [SerializeField] [Range(0f, 360f)] private float rotation = 0f;
    
    [Header("Animation Settings")]
    [SerializeField] private bool animateIntensity = true;
    [SerializeField] [Range(0f, 2f)] private float intensitySpeed = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float intensityAmount = 0.3f;
    
    [SerializeField] private bool animateRotation = false;
    [SerializeField] [Range(-10f, 10f)] private float rotationSpeed = 2f;
    
    private Material material;
    private float baseIntensity;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("GodrayController: No SpriteRenderer found on this object!");
            return;
        }

        material = spriteRenderer.material;
        baseIntensity = intensity;
        UpdateMaterial();
    }

    private void Update()
    {
        if (material == null) return;

        if (animateIntensity)
        {
            float pulse = Mathf.Sin(Time.time * intensitySpeed) * 0.5f + 0.5f;
            intensity = baseIntensity * (1f - intensityAmount + pulse * intensityAmount);
        }

        if (animateRotation)
        {
            rotation += rotationSpeed * Time.deltaTime;
            if (rotation > 360f) rotation -= 360f;
        }

        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if (material == null) return;

        material.SetColor("_RayColor", rayColor);
        material.SetFloat("_Intensity", intensity);
        material.SetFloat("_Rotation", rotation * Mathf.Deg2Rad);
    }

    /// <summary>
    /// Set the godray color. Use HDR colors for bright glowing effects.
    /// </summary>
    public void SetColor(Color newColor)
    {
        rayColor = newColor;
    }

    /// <summary>
    /// Set the godray intensity.
    /// </summary>
    public void SetIntensity(float newIntensity)
    {
        baseIntensity = Mathf.Clamp01(newIntensity);
        intensity = baseIntensity;
    }

    /// <summary>
    /// Rotate the godray effect.
    /// </summary>
    public void SetRotation(float newRotation)
    {
        rotation = newRotation % 360f;
    }

    /// <summary>
    /// Enable/disable intensity pulsing animation.
    /// </summary>
    public void SetAnimateIntensity(bool animate)
    {
        animateIntensity = animate;
    }

    /// <summary>
    /// Enable/disable rotation animation.
    /// </summary>
    public void SetAnimateRotation(bool animate)
    {
        animateRotation = animate;
    }
}
