using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{

    //OTHER SCRIPTS
    private SunController sunController;
    private HealthDataReader healthDataReader;
    private PlayerFindsClosest playerFindsClosest;
    private AudioSourceBehaviour audioSourceBehaviour;
    private ParticleSystemController particleActivator;
    private VegetationBehaviour vegetationBehaviour;
    private FogController fogController;

    //COLORS
    private Color angry = new Color(0.282353f, 0.1019515f, 0.03921569f, 1f);
    private Color sad = new Color(0.0392157f, 0.1106551f, 0.1137255f, 1f);
    private Color happy = new Color(0.0392157f, 0.212f, 0.04367118f, 0.5f);
    private Color stress = new Color(0.2544924f, 0.03921569f, 0.282353f, 1f);
    private Color anxiety = new Color(0.07797226f, 0.0392157f, 0.1137255f, 1f);

    //PARTICLE SYSTEMS
    private bool particleSystemRunning = false;
    private ParticleSystem actualPS;

    //STATES
    public string currentState = "neutral";
    private bool isChangingState = false; // To control if the state can be changed

    //DATA READER
    private object[,] data;
    private int bpmValue = 0;
    private int row = 0;

    public bool automaticMode;

    void Start()
    {
        automaticMode = false;
        Debug.Log("Automatic Mode deactivated");

        //Init object scripts
        playerFindsClosest = FindAnyObjectByType<PlayerFindsClosest>();
        if (playerFindsClosest == null) Debug.Log("PlayerFindsClosest Script not found.");

        sunController = FindAnyObjectByType<SunController>();
        if (sunController == null) Debug.Log("SunController Script not found.");

        healthDataReader = FindAnyObjectByType<HealthDataReader>();
        if (healthDataReader == null) Debug.Log("HealthDataReader Script not found.");
        
        audioSourceBehaviour = FindAnyObjectByType<AudioSourceBehaviour>();
        if (audioSourceBehaviour == null) Debug.Log("AudioSourceBehaviour Script not found.");

        particleActivator = FindAnyObjectByType<ParticleSystemController>();
        if (particleActivator == null) Debug.Log("ParticleActivator Script not found.");

        fogController = FindAnyObjectByType<FogController>();
        if (particleActivator == null) Debug.Log("FogController Script not found.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            automaticMode = !automaticMode;

            Debug.Log(automaticMode ? "Automatic Mode Activated" : "Automatic Mode Deactivated");

            particleActivator.DeactivateAllParticleSystems();
            fogController.DisableFog();

        }
    }

    //Function for Automatic Mode
    public void ChangeState() 
    {
        if (healthDataReader != null)
        {
            data = healthDataReader.returnData(); //Returns XML file values
        }

        //Check if exist particle systems
        if (particleActivator.particleSystems.Length < 0)
        {
            Debug.LogError("Particle systems not found.");
        }
        else
        {
            actualPS = null;
        }

        healthDataReader = FindAnyObjectByType<HealthDataReader>();

        if (isChangingState) return; // If the state is changing, don't do anything

        //Read rows XML file
        while(row < data.GetLength(0) && !isChangingState) {

            bpmValue = (int)data[row, 2];
            
            isChangingState = true; // Indicate that the state is being changed
            particleSystemRunning = true;

            SwitchStateOnBPM(bpmValue);     //Changes state

            // Stop the current particle system if it's active
            if (actualPS != null && actualPS.isPlaying) 
            {
                actualPS.Stop();
            }


            sunController.SetLightState(currentState);  //Change light
            particleActivator.DeactivateAllParticleSystems();   //Deactivate all particle systems

            switch (currentState)
            {
                case "happy":
                    
                    audioSourceBehaviour.StartCoroutine(audioSourceBehaviour.FadeOutAndChangeMusic(2));

                    foreach (VegetationBehaviour actualVegetation in playerFindsClosest.allVegetation)
                    {
                        if (actualVegetation.vegetationType == "Plant")
                        {
                            actualVegetation.actualMesh = "HappyPlant";
                        }

                        else if (actualVegetation.vegetationType == "Tree")
                            actualVegetation.actualMesh = "HappyTree";

                        actualVegetation.actualState = currentState;
                        actualVegetation.alreadyChanged = false;

                        actualVegetation.endColor = happy;

                        audioSourceBehaviour.StopRaindAudio(); 
                    }
                    break;

                case "sad":

                    audioSourceBehaviour.StartCoroutine(audioSourceBehaviour.FadeOutAndChangeMusic(1));
                    audioSourceBehaviour.StartCoroutine(audioSourceBehaviour.FadeOutAndChangeRainAudio(0));
                    
                    foreach (VegetationBehaviour actualVegetation in playerFindsClosest.allVegetation)
                    {
                        if (actualVegetation.vegetationType == "Plant")
                        {
                            actualVegetation.actualMesh = "SadPlant";
                        }

                        else if (actualVegetation.vegetationType == "Tree")
                            actualVegetation.actualMesh = "SadTree";

                        actualVegetation.actualState = currentState;
                        actualVegetation.alreadyChanged = false;

                        actualVegetation.endColor = sad;
                    }

                    break;
                
                case "neutral":

                    audioSourceBehaviour.StartCoroutine(audioSourceBehaviour.FadeOutAndChangeMusic(0));

                    foreach (VegetationBehaviour actualVegetation in playerFindsClosest.allVegetation)
                    {
                        if (actualVegetation.vegetationType == "Plant")
                        {
                            actualVegetation.actualMesh = "NeutralPlant";
                        }
                        else if (actualVegetation.vegetationType == "Tree")
                            actualVegetation.actualMesh = "NeutralTree";

                        actualVegetation.actualState = currentState;
                        actualVegetation.alreadyChanged = false;

                        actualVegetation.endColor = actualVegetation.neutral;

                        audioSourceBehaviour.StopRaindAudio();
                    }

                    break;

                case "angry":

                    audioSourceBehaviour.StartCoroutine(audioSourceBehaviour.FadeOutAndChangeMusic(3));
                    audioSourceBehaviour.StartCoroutine(audioSourceBehaviour.FadeOutAndChangeRainAudio(1));

                    foreach (VegetationBehaviour actualVegetation in playerFindsClosest.allVegetation)
                    {
                        if (actualVegetation.vegetationType == "Plant")
                        {
                            actualVegetation.actualMesh = "AngryPlant";
                        }

                        else if (actualVegetation.vegetationType == "Tree")
                            actualVegetation.actualMesh = "AngryTree";

                        actualVegetation.actualState = currentState;
                        actualVegetation.alreadyChanged = false;

                        actualVegetation.endColor = angry;
                    }

                    break;

                case "stress":

                    audioSourceBehaviour.StartCoroutine(audioSourceBehaviour.FadeOutAndChangeMusic(4));
                    audioSourceBehaviour.StartCoroutine(audioSourceBehaviour.FadeOutAndChangeRainAudio(2));

                    foreach (VegetationBehaviour actualVegetation in playerFindsClosest.allVegetation)
                    {
                        if (actualVegetation.vegetationType == "Plant")
                        {
                            actualVegetation.actualMesh = "StressPlant";
                        }

                        else if (actualVegetation.vegetationType == "Tree")
                            actualVegetation.actualMesh = "StressTree";

                        actualVegetation.actualState = currentState;
                        actualVegetation.alreadyChanged = false;

                        actualVegetation.endColor = stress;
                    }
                    break;

                case "anxiety":

                    audioSourceBehaviour.StartCoroutine(audioSourceBehaviour.FadeOutAndChangeMusic(5));
                    audioSourceBehaviour.StartCoroutine(audioSourceBehaviour.FadeOutAndChangeRainAudio(3));

                    foreach (VegetationBehaviour actualVegetation in playerFindsClosest.allVegetation)
                    {
                       
                        if (actualVegetation.vegetationType == "Plant")
                        {
                            actualVegetation.actualMesh = "AnxiousPlant";
                        }

                        else if (actualVegetation.vegetationType == "Tree")
                            actualVegetation.actualMesh = "AnxiousTree";

                        actualVegetation.actualState = currentState;
                        actualVegetation.alreadyChanged = false;

                        actualVegetation.endColor = anxiety;
                    }

                    break;
            }

            row++;

            if (row >= data.GetLength(0))
            {
                row = 0;
            }

            particleActivator.ActivateParticleSystemsForState(currentState);
            particleActivator.ChangeStormSystem(currentState);
            fogController.ChangeFog(currentState);
            StartCoroutine(WaitBeforeNextChange());
        }
    }
    private void SwitchStateOnBPM(int bpmValue)
    {
        if ((50 <= bpmValue) && (bpmValue <= 59))
            currentState = "sad";
        else if ((60 <= bpmValue) && (bpmValue <= 79))
            currentState = "happy";
        else if ((80 <= bpmValue) && (bpmValue <= 89))
            currentState = "neutral";
        else if ((90 <= bpmValue) && (bpmValue <= 109))
            currentState = "angry";
        else if ((110 <= bpmValue) && (bpmValue <= 129))
            currentState = "stress";
        else
            currentState = "anxiety";

        Debug.Log("Actual BPM: " + bpmValue);
    }

    private IEnumerator WaitBeforeNextChange()
    {
        isChangingState = true; // Indicate that the state is being changed
        yield return new WaitForSeconds(10f); // Wait for 20 seconds
        isChangingState = false; // Allow the state change again

        // Stop the current particle system after the wait

        if (actualPS != null && actualPS.isPlaying)
        {
            actualPS.Stop();
        }

        particleSystemRunning = false;
    }
}
