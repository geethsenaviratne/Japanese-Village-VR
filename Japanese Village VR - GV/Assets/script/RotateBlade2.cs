using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeReveal : MonoBehaviour
{
    // How close the player needs to be to reveal the blade
    public float revealDistance = 5f;

    // Reference to the player
    private GameObject player;

    // Renderer to show/hide the blade
    private Renderer bladeRenderer;

    // Light component for illumination
    private Light bladeLight;

    // Is the blade revealed?
    private bool isRevealed = false;

    void Start()
    {
        // Find the player
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure Player has 'Player' tag.");
        }
        else
        {
            Debug.Log("Player found: " + player.name);
        }

        // Get the renderer component
        bladeRenderer = GetComponent<Renderer>();

        if (bladeRenderer == null)
        {
            Debug.LogError("Renderer not found on Blade!");
        }
        else
        {
            Debug.Log("Renderer found, hiding blade");
            bladeRenderer.enabled = false;
        }

        // Create a light for illumination
        bladeLight = gameObject.AddComponent<Light>();
        bladeLight.type = LightType.Point;
        bladeLight.color = Color.yellow;
        bladeLight.intensity = 2f;
        bladeLight.range = 10f;
        bladeLight.enabled = false;
    }

    void Update()
    {
        // If blade is already revealed, no need to check distance
        if (isRevealed) return;

        // Check if player exists
        if (player == null) return;

        // Calculate distance between player and blade
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Debug the distance
        Debug.Log("Distance to blade: " + distance);

        // If player is close enough, reveal the blade
        if (distance <= revealDistance)
        {
            Debug.Log("Player close enough! Revealing blade!");
            RevealBlade();
        }
    }

    void RevealBlade()
    {
        isRevealed = true;
        Debug.Log("RevealBlade called!");

        // Show the blade
        if (bladeRenderer != null)
        {
            bladeRenderer.enabled = true;
            Debug.Log("Blade renderer enabled");
        }

        // Turn on the light
        if (bladeLight != null)
        {
            bladeLight.enabled = true;
            Debug.Log("Blade light enabled");
        }

        // Optional: Add a glowing effect to the material
        if (bladeRenderer != null && bladeRenderer.material != null)
        {
            bladeRenderer.material.EnableKeyword("_EMISSION");
            bladeRenderer.material.SetColor("_EmissionColor", Color.yellow * 0.5f);
        }
    }
}