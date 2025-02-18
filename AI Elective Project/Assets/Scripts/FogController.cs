using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{

    public Color darkGray = new Color(0.3584906f, 0.3584906f, 0.3584906f, 1f);
    public Color lightGray = new Color(0.4842767f, 0.4842767f, 0.4842767f, 1f);

    public bool enableFog = false; // Controls whether the fog is enabled or disabled.
    public Color fogColor;
    public float fogDensity = 0.02f; // Fog density (adjust as needed).
    private string state = "neutral";


    void Start()
    {
        // Initially, we set the fog to be disabled.
        RenderSettings.fog = false;
    }

    void Update()
    {
        if (enableFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
        }
    }

    // You can call this method from another script to enable the fog.
    public void ToggleFog(bool isEnabled)
    {
        enableFog = isEnabled;
    }

    public void DisableFog()
    {
        RenderSettings.fog = false;
        enableFog = false;
    }

    // Method to adjust the fog density.
    public void SetFogDensity(float density)
    {
        fogDensity = density;
    }

    // Method to adjust the fog color.
    public void SetFogColor(Color color)
    {
        fogColor = color;
    }

    public void ChangeFog(string state)
    {
        switch (state)
        {
            case "angry":
                enableFog = true;
                fogColor = lightGray;
                fogDensity = 0.03f;
                break;
            case "stress":
                enableFog = true;
                fogColor = Color.gray;
                fogDensity = 0.07f;
                break;
            case "anxiety":
                enableFog = true;
                fogColor = darkGray;
                fogDensity = 0.12f;
                break;
            default:
                RenderSettings.fog = false;
                enableFog = false;
                break;
        }
    }
}
