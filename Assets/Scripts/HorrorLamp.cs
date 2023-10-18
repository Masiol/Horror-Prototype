using UnityEngine;

public class HorrorLamp : MonoBehaviour
{
    public Light lampLight;
    public Material lampMaterial;
    public float minIntensity = 0.2f;
    public float maxIntensity = 1f;
    public Color minEmissionColor = Color.black;
    public Color maxEmissionColor = Color.white;
    public float minFlickerDelay = 0.1f;
    public float maxFlickerDelay = 1f;
    public float intensityChangeSpeed = 1f;

    private float targetIntensity;
    private float currentIntensity;
    private Color targetEmissionColor;
    private Color currentEmissionColor;
    private float flickerDelay;
    private float flickerTimer;

    private void Start()
    {
        targetIntensity = lampLight.intensity;
        currentIntensity = targetIntensity;
        targetEmissionColor = lampMaterial.GetColor("_EmissionColor");
        currentEmissionColor = targetEmissionColor;
        flickerDelay = Random.Range(minFlickerDelay, maxFlickerDelay);
        flickerTimer = flickerDelay;
    }

    private void Update()
    {
        flickerTimer -= Time.deltaTime;

        if (flickerTimer <= 0f)
        {
            flickerTimer = Random.Range(minFlickerDelay, maxFlickerDelay);
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            targetEmissionColor = Random.ColorHSV();
        }

        currentIntensity = Mathf.MoveTowards(currentIntensity, targetIntensity, intensityChangeSpeed * Time.deltaTime);
        lampLight.intensity = currentIntensity;

        currentEmissionColor = Color.Lerp(currentEmissionColor, targetEmissionColor, intensityChangeSpeed * Time.deltaTime);
        lampMaterial.SetColor("_EmissionColor", currentEmissionColor);
    }
}