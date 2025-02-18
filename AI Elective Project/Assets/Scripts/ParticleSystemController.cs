using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    private FogController fogController;
    private EnvironmentController environmentController;

    public ParticleSystem[] particleSystems;
    public KeyCode[] particleKeys;
    private bool particleSystemRunning = false;
    private ParticleSystem actualPS;
    public ParticleSystem cloudsPS;
    public ParticleSystem thunderPS;

    // Colors
    private Color orange = new Color(1f, 0.4604093f, 0f, 1f);
    private Color purple = new Color(0.6231885f, 0.3250464f, 1f, 1f);
    private Color darkGray = new Color(0.2012578f, 0.2012578f, 0.2012578f, 1f);
    private Color mediumGray = new Color(0.2327043f, 0.2327043f, 0.2327043f, 1f);
    private Color lightGray = new Color(0.553459f, 0.553459f, 0.553459f, 1f);
    private Color gray = Color.gray;
    private Color cloudsColor;

    public Material thunderMaterial;
    public Material fireMaterial;

    private Color thunderColor;
    private Color fireColor;
    private Color thunderOriginalColor;
    private Color fireOriginalColor;

    private float linearXVelocity;
    private int startLifeTimeOn;
    private int simulationSpeedOn;
    public string state;

    private void Start()
    {

        fogController = FindAnyObjectByType<FogController>();
        if (fogController == null) Debug.Log("FogController Script not found.");

        environmentController = FindAnyObjectByType<EnvironmentController>();
        if (environmentController == null) Debug.Log("EnvironmentController Script not found");

        if (particleSystems.Length < 0)
        {
            Debug.LogError("No particle systems found.");
        }
        else
        {
            actualPS = particleSystems[0];
            for (int i = 0; i < particleKeys.Length; i++)
            {
                particleSystems[i].Stop();
            }
        }

        thunderOriginalColor = thunderMaterial.color;

        cloudsPS.Stop();
        var main = cloudsPS.main;
        main.startColor = lightGray;
        thunderPS.Stop();

    }
    void Update()
    {
        if (!environmentController.automaticMode)
        {
            for (int i = 0; i < particleKeys.Length; i++)
            {
                if (Input.GetKeyDown(particleKeys[i]))
                {
                    if (particleKeys[i] == KeyCode.Keypad1)
                    {
                        state = "sad";
                        ChangeStormSystem(state);
                    }
                    else if (particleKeys[i] == KeyCode.Keypad4)
                    {
                        state = "angry";
                        ChangeStormSystem(state);
                    }
                    else if (particleKeys[i] == KeyCode.Keypad5)
                    {
                        state = "stress";
                        ChangeStormSystem(state);
                    }
                    else if (particleKeys[i] == KeyCode.Keypad6)
                    {
                        state = "anxiety";
                        ChangeStormSystem(state);
                    }

                    else
                    {
                        DeactivateStormSystem();
                    }

                    fogController.ChangeFog(state);

                    actualPS.Stop();

                    actualPS = particleSystems[i];
                    particleSystems[i].Play();
                    particleSystemRunning = true;

                }

                if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    DeactivateAllParticleSystems();
                }
            }
        }
    }

    public void ActivateParticleSystemsForState(string state)
    {
        foreach (ParticleSystem system in particleSystems)
        {
            if (system.name.Contains(state))
            {
                system.Play();
            }
        }
    }
    public void DeactivateAllParticleSystems()
    {
        foreach (ParticleSystem system in particleSystems)
        {
            system.Stop();
        }

        DeactivateStormSystem();
    }
    public void ChangeStormSystem(string state)
    {
        if (state == "happy" || state == "neutral")
        {
            DeactivateStormSystem();

        }
        else
        {
            ActivateStormSystem(state);
        }
    }
    public void ActivateStormSystem(string state)
    {
        ChangeCloudsSystem(state);
        cloudsPS.Clear();
        cloudsPS.Play();

        ChangeThundersSystem(state);

        if (state != "sad")
        {
            thunderPS.Stop();
            thunderPS.Clear();
            thunderPS.Play();
        }
    }
    public void DeactivateStormSystem()
    {
        cloudsPS.Stop();
        cloudsPS.Clear();
        thunderPS.Stop();
        thunderPS.Clear();
    }
    public void ChangeCloudsSystem(string state)
    {
        var velocityOverLifetime = cloudsPS.velocityOverLifetime;
        velocityOverLifetime.enabled = true;

        // Set default values
        startLifeTimeOn = 1;
        linearXVelocity = 0; // Default initial velocity

        switch (state)
        {
            case "sad":
                startLifeTimeOn = 100;
                linearXVelocity = 1;
                cloudsColor = lightGray;
                break;
            case "angry":
                linearXVelocity = 100;
                cloudsColor = gray;
                break;
            case "stress":
                linearXVelocity = 200;
                cloudsColor = mediumGray;
                break;
            case "anxiety":
                linearXVelocity = 300;
                cloudsColor = darkGray;
                break;
            default:
                velocityOverLifetime.enabled = false; // Disable velocity for other states
                break;
        }

        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(linearXVelocity);
        var main = cloudsPS.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(startLifeTimeOn);
        main.startColor = cloudsColor;
    }

    public void ChangeThundersSystem(string state)
    {
        var thunderMain = thunderPS.main;
        var simulationSpeed = thunderPS.main.simulationSpeed;
        var maxParticles = thunderMain.maxParticles;

        if (state == "angry")
        {
            simulationSpeedOn = 3;
            maxParticles = 3;
            thunderColor = orange;
        }

        else if (state == "stress")
        {
            simulationSpeedOn = 5;
            maxParticles = 5;
            thunderColor = purple;
        }

        else if (state == "anxiety")
        {
            simulationSpeedOn = 9;
            maxParticles = 9;
            thunderColor = thunderOriginalColor;
        }

        thunderMaterial.EnableKeyword("_EMISSION");
        thunderMaterial.SetColor("_EmissionColor", thunderColor);

        thunderMain.simulationSpeed = simulationSpeedOn;
        thunderMain.maxParticles = maxParticles;
    }
}
