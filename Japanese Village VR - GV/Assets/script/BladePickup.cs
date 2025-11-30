using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BladePickup : MonoBehaviour
{
    [Header("Reveal Settings")]
    public float revealDistance = 5f;

    [Header("Pickup Settings")]
    public float pickupDistance = 3f;
    public KeyCode pickupKey = KeyCode.E;

    [Header("References")]
    public TextMeshProUGUI messageText;
    public Transform handPosition;

    // Private variables
    private GameObject player;
    private Renderer bladeRenderer;
    private Light bladeLight;
    private Collider bladeCollider;

    private bool isRevealed = false;
    private bool isPickedUp = false;

    // Original blade settings
    private Vector3 originalScale;
    private Quaternion originalRotation;

    void Start()
    {
        // Find the player
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure Player has 'Player' tag.");
            return;
        }

        // Find hand position (child of player)
        if (handPosition == null)
        {
            GameObject handObj = GameObject.Find("HandPosition");
            if (handObj != null)
            {
                handPosition = handObj.transform;
            }
            else
            {
                Debug.LogError("HandPosition not found!");
            }
        }

        // Find message text
        if (messageText == null)
        {
            GameObject textObj = GameObject.Find("MessageText");
            if (textObj != null)
            {
                messageText = textObj.GetComponent<TextMeshProUGUI>();
            }
        }

        // Get components
        bladeRenderer = GetComponent<Renderer>();
        bladeCollider = GetComponent<Collider>();

        // Save original settings
        originalScale = transform.localScale;
        originalRotation = transform.rotation;

        // Hide blade at start
        if (bladeRenderer != null)
        {
            bladeRenderer.enabled = false;
        }

        // Create a light
        bladeLight = gameObject.AddComponent<Light>();
        bladeLight.type = LightType.Point;
        bladeLight.color = Color.yellow;
        bladeLight.intensity = 3f;
        bladeLight.range = 10f;
        bladeLight.enabled = false;

        // Hide message
        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    void Update()
    {
        if (player == null) return;

        // Calculate distance
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Step 1: Reveal blade when player is close
        if (!isRevealed && distance <= revealDistance)
        {
            RevealBlade();
        }

        // Step 2: Show pickup prompt and allow pickup
        if (isRevealed && !isPickedUp)
        {
            if (distance <= pickupDistance)
            {
                // Show pickup message
                if (messageText != null)
                {
                    messageText.text = "Press E to pick up the blade";
                }

                // Check for pickup key
                if (Input.GetKeyDown(pickupKey))
                {
                    PickupBlade();
                }
            }
            else
            {
                // Clear message when too far
                if (messageText != null)
                {
                    messageText.text = "";
                }
            }
        }

        // Step 3: Keep blade in hand position
        if (isPickedUp && handPosition != null)
        {
            transform.position = handPosition.position;
            transform.rotation = handPosition.rotation;
        }
    }

    void RevealBlade()
    {
        isRevealed = true;
        Debug.Log("Blade revealed!");

        // Show the blade
        if (bladeRenderer != null)
        {
            bladeRenderer.enabled = true;
        }

        // Turn on light
        if (bladeLight != null)
        {
            bladeLight.enabled = true;
        }

        // Show discovery message briefly
        if (messageText != null)
        {
            messageText.text = "You found the blade!";
            Invoke("ClearMessage", 2f);
        }
    }

    void PickupBlade()
    {
        isPickedUp = true;
        Debug.Log("Blade picked up!");

        // Turn off the world light (since it's in hand now)
        if (bladeLight != null)
        {
            bladeLight.enabled = false;
        }

        // Disable collider so it doesn't interfere
        if (bladeCollider != null)
        {
            bladeCollider.enabled = false;
        }

        // Parent to hand position
        if (handPosition != null)
        {
            transform.SetParent(handPosition);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            // Adjust scale if needed
            transform.localScale = originalScale * 0.8f;
        }

        // Update message
        if (messageText != null)
        {
            messageText.text = "Return the blade to the village center!";
            Invoke("ClearMessage", 3f);
        }
    }

    void ClearMessage()
    {
        if (messageText != null && !isPickedUp)
        {
            messageText.text = "";
        }
    }

    public bool HasBlade()
    {
        return isPickedUp;
    }
}