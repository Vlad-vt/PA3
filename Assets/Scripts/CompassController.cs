using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompassController : MonoBehaviour
{
    public float compassSmooth = 3f; // Smoothing factor
    private float m_lastMagneticHeading = 0f;

    public Button enableMagnetometer;
    public TextMeshProUGUI magnetometerValue;

    private TextMeshProUGUI enableMagnetometerButtonText;

    void Start()
    {
        // If you need an accurate heading to true north,
        // start the location service so Unity can correct for local deviations:
        Input.location.Start();
        // Start the compass.
        Input.compass.enabled = true;
        enableMagnetometer.onClick.AddListener(ToggleMagnetometer);
        enableMagnetometerButtonText = enableMagnetometer.GetComponentInChildren<TextMeshProUGUI>();
        enableMagnetometerButtonText.text = "Enabled";

        // Cache the current compass value
        m_lastMagneticHeading = Input.compass.magneticHeading;
    }

    // Update is called once per frame
    private void Update()
    {
        try
        {
            // Check if the magnetometer is enabled
            if (Input.compass.enabled)
            {
                // Update the UI text with the current magnetometer value
                magnetometerValue.text = Input.compass.magneticHeading.ToString("0.");

                // Smoothly interpolate between current rotation and target rotation
                Quaternion targetRotation = Quaternion.Euler(0, Input.compass.magneticHeading, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, compassSmooth * Time.deltaTime);

                // Check if the compass value has changed significantly
                if (Math.Abs(Input.compass.magneticHeading - m_lastMagneticHeading) > compassSmooth)
                {
                    // Update the last magnetometer value
                    m_lastMagneticHeading = Input.compass.magneticHeading;
                }
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void ToggleMagnetometer()
    {
        if (Input.compass.enabled)
        {
            DisableMagnetometer();
        }
        else
        {
            EnableMagnetometer();
        }
    }

    private void EnableMagnetometer()
    {
        Input.compass.enabled = true;
        enableMagnetometerButtonText.text = "Enabled";
    }

    private void DisableMagnetometer()
    {
        Input.compass.enabled = false;
        enableMagnetometerButtonText.text = "Disabled";
    }
}