using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractiveStatue : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI messageText;

    [Header("Settings")]
    public float detectionRadius = 5f;
    public bool requireLanternLit = true;
    public KeyCode interactionKey = KeyCode.E;

    [Header("Rotation Settings")]
    public float rotationSpeed = 30f; // Degrees per second for continuous rotation
    public Vector3 rotationAxis = Vector3.up; // Rotate around Y axis (up)

    [Header("Messages")]
    public string approachMessage = "Press E to activate the statue";
    public string rotatingMessage = "The statue is rotating...";
    public string completedMessage = "Look inside the shrine";
    public float messageDisplayTime = 3f; // How long to show the completion message

    [Header("Audio")]
    public AudioClip rotationSound; // Stone grinding/rotating sound

    private GameObject player;
    private bool isActivated = false;
    private bool playerNearby = false;
    private AudioSource audioSource;
    private InteractiveLantern lantern;

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

        // Find the interactive lantern to check if it's been lit
        if (requireLanternLit)
        {
            lantern = FindObjectOfType<InteractiveLantern>();
        }

        // Setup audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false; // Loop the sound forever

        if (rotationSound != null)
        {
            audioSource.clip = rotationSound;
        }

        Debug.Log("Interactive Statue initialized");
    }

    void Update()
    {
        if (player == null) return;

        // If statue is activated, rotate it continuously
        if (isActivated)
        {
            RotateContinuously();
        }

        // Check player distance and input
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= detectionRadius)
        {
            if (!playerNearby)
            {
                playerNearby = true;
                if (!isActivated)
                {
                    ShowApproachMessage();
                }
            }

            // Check if player presses E (only if not already activated)
            if (!isActivated && Input.GetKeyDown(interactionKey))
            {
                // Check if lantern requirement is met
                if (requireLanternLit && lantern != null)
                {
                    // Check if lantern is lit (if you have the IsLit() method)
                    // For now, just activate
                    ActivateStatue();
                }
                else if (!requireLanternLit)
                {
                    ActivateStatue();
                }
            }
        }
        else
        {
            if (playerNearby && !isActivated)
            {
                playerNearby = false;
                ClearMessage();
            }
        }
    }

    void RotateContinuously()
    {
        // Rotate the statue every frame
        float rotationThisFrame = rotationSpeed * Time.deltaTime;
        transform.Rotate(rotationAxis, rotationThisFrame, Space.World);
    }

    void ShowApproachMessage()
    {
        if (messageText != null)
        {
            messageText.text = approachMessage;
        }
    }

    void ActivateStatue()
    {
        if (isActivated) return;

        isActivated = true;
        Debug.Log("Statue activated - rotating forever!");

        // Show rotating message
        if (messageText != null)
        {
            messageText.text = rotatingMessage;
        }

        // Start playing rotation sound (loops forever)
        if (rotationSound != null && audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Playing rotation sound (looping)");
        }

        // Show completion message after a delay
        Invoke("ShowCompletionMessage", messageDisplayTime);

        // Optional: Reveal the blade or secret
        RevealSecret();
    }

    void ShowCompletionMessage()
    {
        if (messageText != null)
        {
            messageText.text = completedMessage;
            Invoke("ClearMessage", 5f);
        }
    }

    void RevealSecret()
    {
        // You can add code here to reveal the blade or open a passage
        // For example:
        /*
        GameObject blade = GameObject.Find("Blade");
        if (blade != null)
        {
            blade.SetActive(true);
            
            // Enable blade's glow/reveal effect
            BladePickup bladeScript = blade.GetComponent<BladePickup>();
            if (bladeScript != null)
            {
                bladeScript.enabled = true;
            }
            
            Debug.Log("Blade revealed!");
        }
        */

        Debug.Log("Secret revealed!");
    }

    void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    // Optional: Method to stop rotation (if needed later)
    public void StopRotation()
    {
        isActivated = false;

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        Debug.Log("Statue rotation stopped");
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw rotation axis
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, rotationAxis.normalized * 3f);

        // Draw rotation direction indicator
        if (Application.isPlaying && isActivated)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}