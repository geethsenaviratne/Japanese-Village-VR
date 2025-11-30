using System.Collections;
using UnityEngine;
using TMPro;

public class InteractiveBook : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI messageText;

    [Header("Settings")]
    public float detectionRadius = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Background Illumination")]
    public GameObject backgroundPlane; // The surface/platform under the book
    public Color glowColor = new Color(1f, 0.9f, 0.6f); // Warm golden glow
    public float glowIntensity = 3f;
    public float lightIntensity = 5f;
    public float lightRange = 8f;
    public bool pulseGlow = true;
    public float pulseSpeed = 2f;

    [Header("Book Rise Effect")]
    public float riseHeight = 0.5f; // How high the book rises
    public float riseSpeed = 2f; // How fast it rises

    private GameObject player;
    private bool playerNearby = false;
    private bool clueRevealed = false;
    private Light backgroundLight;
    private float baseLightIntensity;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isRising = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (messageText == null)
        {
            GameObject textObj = GameObject.Find("MessageText");
            if (textObj != null)
                messageText = textObj.GetComponent<TextMeshProUGUI>();
        }

        // Store original position
        originalPosition = transform.position;
        targetPosition = originalPosition;

        // Create background light
        CreateBackgroundLight();

        // Enable glow on background only
        if (backgroundPlane != null)
        {
            EnableBackgroundGlow();
        }
        else
        {
            Debug.LogWarning("Background plane not assigned! Please assign the platform/surface under the book.");
        }

        ClearMessage();
    }

    void CreateBackgroundLight()
    {
        // Create a light under/around the book for background illumination
        GameObject lightObj = new GameObject("BackgroundLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = Vector3.down * 0.2f; // Below the book, lights the ground

        backgroundLight = lightObj.AddComponent<Light>();
        backgroundLight.type = LightType.Point;
        backgroundLight.color = glowColor;
        backgroundLight.intensity = lightIntensity;
        backgroundLight.range = lightRange;
        backgroundLight.shadows = LightShadows.None;

        baseLightIntensity = lightIntensity;

        Debug.Log("Background light created!");
    }

    void EnableBackgroundGlow()
    {
        // Get all renderers in the background plane
        Renderer[] renderers = backgroundPlane.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            // Try the background plane itself
            Renderer bgRenderer = backgroundPlane.GetComponent<Renderer>();
            if (bgRenderer != null)
            {
                ApplyGlowToMaterial(bgRenderer.material);
            }
        }
        else
        {
            foreach (Renderer renderer in renderers)
            {
                if (renderer.material != null)
                {
                    ApplyGlowToMaterial(renderer.material);
                }
            }
        }

        Debug.Log("Background glow enabled!");
    }

    void ApplyGlowToMaterial(Material mat)
    {
        // Enable emission
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", glowColor * glowIntensity);

        // If there's no emission map, use the base map
        if (mat.GetTexture("_EmissionMap") == null)
        {
            Texture baseMap = mat.GetTexture("_BaseMap");
            if (baseMap != null)
            {
                mat.SetTexture("_EmissionMap", baseMap);
            }
        }

        // Enable global illumination
        mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

        Debug.Log("Applied glow to material: " + mat.name);
    }

    void Update()
    {
        if (player == null)
            return;

        // Pulse the background light for magical effect
        if (pulseGlow && backgroundLight != null && !clueRevealed)
        {
            float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.3f + 1f;
            backgroundLight.intensity = baseLightIntensity * pulse;
        }

        // Smoothly move book to target position
        if (isRising)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * riseSpeed);

            // Check if reached target
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isRising = false;
            }
        }

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= detectionRadius)
        {
            playerNearby = true;

            if (!clueRevealed)
            {
                ShowMessage("Press E to reveal clue");

                if (Input.GetKeyDown(interactKey))
                {
                    RevealClue();
                }
            }
        }
        else
        {
            playerNearby = false;
            if (!clueRevealed)
            {
                ClearMessage();
            }
        }
    }

    void RevealClue()
    {
        clueRevealed = true;

        // Stop pulsing and brighten the background light
        if (backgroundLight != null)
        {
            backgroundLight.intensity = baseLightIntensity * 1.5f;
        }

        // Make the book rise
        targetPosition = originalPosition + Vector3.up * riseHeight;
        isRising = true;

        // Clear first to force UI update
        ClearMessage();

        // Show the new clue
        ShowMessage("Look under the tree");

        Debug.Log("Clue revealed: Look under the tree - Book rising!");

        // Keep message visible for longer
        Invoke("ClearMessage", 5f);
    }

    void ShowMessage(string text)
    {
        if (messageText != null)
            messageText.text = text;
    }

    void ClearMessage()
    {
        if (messageText != null && clueRevealed)
            messageText.text = "";
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw rise target position
        if (Application.isPlaying && clueRevealed)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(targetPosition, 0.2f);
            Gizmos.DrawLine(originalPosition, targetPosition);
        }
    }
}