using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class SunController : MonoBehaviour
{
    private EnvironmentController environmentController; 

    private Color angry = new Color(1f, 0f, 0f, 1f);
    private Color neutral = new Color(0.6918238f, 0.6847792f, 0.6548395f, 0.5843138f);
    private Color sad = new Color(0.3300502f, 0.4164391f, 0.7044024f, 0.5f);
    private Color happy = new Color(0.2819204f, 0.5220125f, 0.2084766f, 0.5f);
    private Color stress = new Color(0.4923722f, 0.3250464f, 0.5974842f, 0.5f);
    private Color anxiety = new Color(0f, 0.17541f, 0.3522012f, 0.5f);


    public Light sunLight; // Arrastra aquí la luz direccional en el inspector
    public Color sunColor; // Color por defecto del sol
    [Range(0, 5)] // Añade un rango para la intensidad
    public float intensity = 1.0f; // Intensidad por defecto

    public float transitionDuration = 2f; // Tiempo para completar la interpolación

    private float transitionProgress = 0f;
    private bool automaticModeActivated = false;
    private bool isTransitioning = false;

    void Start()
    {
        environmentController = FindAnyObjectByType<EnvironmentController>();
        if (environmentController == null) Debug.Log("EnvironmentController Script nof found");

        if (sunLight == null)
        {
            // Encuentra automáticamente la luz direccional si no se ha asignado
            sunLight = FindObjectOfType<Light>();
            if (sunLight != null && sunLight.type == LightType.Directional)
            {
                sunColor = neutral;
                sunLight.intensity = intensity;
                Debug.Log("Luz direccional encontrada.");
            }
            else
            {
                Debug.LogError("No se encontró una luz direccional en la escena.");
            }
        }

        // Cambia el color y la intensidad inicial
        startTransition(sunColor, intensity);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            automaticModeActivated = !automaticModeActivated;
            Debug.Log(automaticModeActivated ? "Automatic mode Activated" : "Automatic mode Deactivated");
        }

        if (!automaticModeActivated)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1)) { UpdateSunState(sad, 0.5f); }
            if (Input.GetKeyDown(KeyCode.Keypad2)) { UpdateSunState(neutral, 1.5f); }
            if (Input.GetKeyDown(KeyCode.Keypad3)) { UpdateSunState(happy, 2f); }
            if (Input.GetKeyDown(KeyCode.Keypad4)) { UpdateSunState(angry, 5f); }
            if (Input.GetKeyDown(KeyCode.Keypad5)) { UpdateSunState(anxiety, 0.2f); }
            if (Input.GetKeyDown(KeyCode.Keypad6)) { UpdateSunState(stress, 0.2f); }

            // Cambia el color y la intensidad en tiempo real
            startTransition(sunColor, intensity);
        }
    }

    public void startTransition(Color color, float intensity)
    {
        if (sunLight != null)
        {
            sunLight.color = color; // Cambia el color del sol
            sunLight.intensity = intensity; // Cambia la intensidad del sol
        }
    }

    private void UpdateSunState(Color color, float intensity)
    {
        transitionProgress = 0f;
        sunColor = color;
        this.intensity = intensity;
        startTransition(sunColor, intensity);
    }
    public void SetLightState(string state)
    {
        // Lógica para cambiar el estado de la luz dependiendo del estado recibido
        if (state == "sad") UpdateSunState(sad, 0.5f);
        else if (state == "happy") UpdateSunState(happy, 2);
        else if (state == "neutral") UpdateSunState(neutral, 1.5f);
        else if (state == "angry") UpdateSunState(angry, 5f);
        else if (state == "stress") UpdateSunState(stress, 0.2f);
        else if (state == "anxiety") UpdateSunState(anxiety, 0.2f);
    }
}
