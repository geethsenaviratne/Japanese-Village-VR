using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagWind : MonoBehaviour
{
    [Header("Wind Settings")]
    public float windStrength = 1f;
    public float windSpeed = 1f;
    public Vector3 windDirection = new Vector3(1f, 0f, 0f);

    [Header("Wave Settings")]
    public float waveHeight = 0.5f;
    public float waveSpeed = 2f;

    [Header("Randomness")]
    public bool randomizeWind = true;
    public float gustInterval = 3f;
    public float gustStrength = 2f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float gustTimer;
    private float currentGustMultiplier = 1f;
    private float timeOffset;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        if (randomizeWind)
        {
            // FIXED: Explicitly use UnityEngine.Random
            timeOffset = UnityEngine.Random.Range(0f, 100f);
            gustTimer = UnityEngine.Random.Range(0f, gustInterval);
        }
    }

    void Update()
    {
        // Calculate wind effect
        float windEffect = Mathf.Sin((Time.time + timeOffset) * windSpeed) * windStrength * currentGustMultiplier;
        float waveEffect = Mathf.Sin((Time.time + timeOffset) * waveSpeed) * waveHeight;

        // Apply rotation (flag waving)
        Vector3 rotation = new Vector3(
            waveEffect * 10f,  // Pitch (up/down wave)
            windEffect * 20f,  // Yaw (side to side)
            windEffect * 5f    // Roll (slight twist)
        );

        transform.localRotation = originalRotation * Quaternion.Euler(rotation);

        // Apply slight position offset (flag moving with wind)
        Vector3 positionOffset = windDirection.normalized * windEffect * 0.1f;
        transform.localPosition = originalPosition + positionOffset;

        // Handle wind gusts
        if (randomizeWind)
        {
            gustTimer -= Time.deltaTime;

            if (gustTimer <= 0f)
            {
                StartCoroutine(WindGust());
                // FIXED: Explicitly use UnityEngine.Random
                gustTimer = UnityEngine.Random.Range(gustInterval * 0.5f, gustInterval * 1.5f);
            }
        }

        // Gradually return to normal wind
        if (currentGustMultiplier > 1f)
        {
            currentGustMultiplier = Mathf.Lerp(currentGustMultiplier, 1f, Time.deltaTime * 0.5f);
        }
    }

    IEnumerator WindGust()
    {
        // FIXED: Explicitly use UnityEngine.Random
        float targetMultiplier = UnityEngine.Random.Range(gustStrength * 0.8f, gustStrength * 1.2f);
        float duration = UnityEngine.Random.Range(0.5f, 1.5f);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            currentGustMultiplier = Mathf.Lerp(1f, targetMultiplier, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw wind direction
        Gizmos.color = UnityEngine.Color.cyan; // FIXED: Explicitly use UnityEngine.Color
        Gizmos.DrawRay(transform.position, windDirection.normalized * 2f);
    }
}