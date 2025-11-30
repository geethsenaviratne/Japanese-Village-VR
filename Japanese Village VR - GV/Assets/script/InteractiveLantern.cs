using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiveLantern : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI messageText;
    public Material lanternBaseMaterial;
    public Material lanternGlassMaterial;

    [Header("Settings")]
    public KeyCode interactionKey = KeyCode.E;
    public float detectionRadius = 3f;

    [Header("Light Settings")]
    public UnityEngine.Color lanternGlowColor = new UnityEngine.Color(1f, 0.75f, 0.4f);
    public float lightIntensity = 8f;
    public float lightRange = 12f;
    public float emissionIntensity = 5f;

    [Header("Messages")]
    public string approachMessage = "Press E to light the offering";
    public string completionMessage = "Go find the statue";

    [Header("Effects")]
    public ParticleSystem lightingEffect;
    public AudioClip lightingSound;

    [Header("Cherry Blossom Effect")]
    public bool enableCherryBlossoms = true;
    public int numberOfPetals = 50;
    public float petalLifetime = 5f;
    public UnityEngine.Color petalColor = new UnityEngine.Color(1f, 0.8f, 0.9f);
    public float petalSize = 0.3f;
    public float spawnHeight = 5f;
    public float spawnRadius = 3f;

    private GameObject player;
    private bool isLit = false;
    private bool playerNearby = false;
    private Light lanternLight;
    private AudioSource audioSource;
    private ParticleSystem cherryBlossomParticles;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (messageText == null)
        {
            GameObject textObj = GameObject.Find("MessageText");
            if (textObj != null)
            {
                messageText = textObj.GetComponent<TextMeshProUGUI>();
            }
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && lightingSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        lanternLight = GetComponentInChildren<Light>();
        if (lanternLight == null)
        {
            GameObject lightObj = new GameObject("LanternLight");
            lightObj.transform.SetParent(transform);
            lightObj.transform.localPosition = Vector3.zero;
            lanternLight = lightObj.AddComponent<Light>();
        }

        lanternLight.type = LightType.Point;
        lanternLight.color = lanternGlowColor;
        lanternLight.intensity = lightIntensity;
        lanternLight.range = lightRange;
        lanternLight.enabled = false;

        if (enableCherryBlossoms)
        {
            CreateCherryBlossomSystem();
        }

        TurnOffEmission();
    }

    void CreateCherryBlossomSystem()
    {
        GameObject particleObj = new GameObject("CherryBlossomParticles");
        particleObj.transform.SetParent(transform);
        particleObj.transform.localPosition = Vector3.up * spawnHeight;

        cherryBlossomParticles = particleObj.AddComponent<ParticleSystem>();

        var main = cherryBlossomParticles.main;
        main.startLifetime = petalLifetime;
        main.startSpeed = 1f;
        main.startSize = petalSize;
        main.startColor = petalColor;
        main.gravityModifier = 0.2f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = numberOfPetals;
        main.loop = false;
        main.playOnAwake = false;

        var emission = cherryBlossomParticles.emission;
        emission.enabled = true;
        var burst = new ParticleSystem.Burst(0f, numberOfPetals);
        emission.SetBurst(0, burst);

        var shape = cherryBlossomParticles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = spawnRadius;
        shape.radiusThickness = 1f;

        var velocityOverLifetime = cherryBlossomParticles.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.World;
        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(-1f, 1f);
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(-2f, -1f);
        velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(-1f, 1f);

        var rotationOverLifetime = cherryBlossomParticles.rotationOverLifetime;
        rotationOverLifetime.enabled = true;
        rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(-180f, 180f);

        var sizeOverLifetime = cherryBlossomParticles.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0f, 1f);
        sizeCurve.AddKey(0.8f, 1f);
        sizeCurve.AddKey(1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        var colorOverLifetime = cherryBlossomParticles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(petalColor, 0f),
                new GradientColorKey(petalColor, 0.8f),
                new GradientColorKey(UnityEngine.Color.white, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0.8f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        var renderer = cherryBlossomParticles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
        renderer.material.color = petalColor;

        Debug.Log("Cherry blossom particle system created!");
    }

    void Update()
    {
        if (player == null || isLit) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= detectionRadius)
        {
            if (!playerNearby)
            {
                playerNearby = true;
                ShowApproachMessage();
            }

            if (Input.GetKeyDown(interactionKey))
            {
                LightLantern();
            }
        }
        else
        {
            if (playerNearby)
            {
                playerNearby = false;
                ClearMessage();
            }
        }
    }

    void ShowApproachMessage()
    {
        if (messageText != null)
        {
            messageText.text = approachMessage;
        }
    }

    void LightLantern()
    {
        if (isLit) return;

        isLit = true;
        Debug.Log("Lantern lit!");

        if (lanternLight != null)
        {
            lanternLight.enabled = true;
        }

        TurnOnEmission();

        if (lightingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(lightingSound);
        }

        if (lightingEffect != null)
        {
            lightingEffect.Play();
        }

        if (enableCherryBlossoms && cherryBlossomParticles != null)
        {
            cherryBlossomParticles.Play();
            Debug.Log("Cherry blossoms falling!");
        }

        if (messageText != null)
        {
            messageText.text = completionMessage;
            Invoke("ClearMessage", 5f);
        }

        StartCoroutine(LanternCelebration());
    }

    IEnumerator LanternCelebration()
    {
        float originalIntensity = lightIntensity;

        for (int i = 0; i < 3; i++)
        {
            if (lanternLight != null)
            {
                lanternLight.intensity = originalIntensity * 1.5f;
            }
            yield return new WaitForSeconds(0.3f);

            if (lanternLight != null)
            {
                lanternLight.intensity = originalIntensity;
            }
            yield return new WaitForSeconds(0.3f);
        }

        if (lanternLight != null)
        {
            lanternLight.intensity = originalIntensity;
        }
    }

    void TurnOnEmission()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (renderer.material != null)
            {
                Material mat = renderer.material;
                string materialName = mat.name.ToLower();

                if (materialName.Contains("glass") || materialName.Contains("lantern_glass"))
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", lanternGlowColor * emissionIntensity);

                    if (mat.GetTexture("_EmissionMap") == null)
                    {
                        Texture baseMap = mat.GetTexture("_BaseMap");
                        if (baseMap != null)
                        {
                            mat.SetTexture("_EmissionMap", baseMap);
                        }
                    }

                    mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

                    Debug.Log("Enabled emission on GLASS: " + renderer.gameObject.name);
                }
                else
                {
                    Debug.Log("Skipped non-glass material: " + materialName);
                }
            }
        }
    }

    void TurnOffEmission()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (renderer.material != null)
            {
                renderer.material.DisableKeyword("_EMISSION");
            }
        }
    }

    void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (enableCherryBlossoms)
        {
            Gizmos.color = new UnityEngine.Color(1f, 0.5f, 0.8f, 0.3f);
            Vector3 spawnPos = transform.position + Vector3.up * spawnHeight;
            Gizmos.DrawWireSphere(spawnPos, spawnRadius);
        }
    }
}