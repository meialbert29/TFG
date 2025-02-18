using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationBehaviour : MonoBehaviour
{
    private Renderer vegetationRenderer;
    private EnvironmentController environmentController;
    private PlayerFindsClosest playerFindsClosest;

    //COLORS
    private Color angry = new Color(0.282353f, 0.1019515f, 0.03921569f, 1f);
    private Color sad = new Color(0.0392157f, 0.1106551f, 0.1137255f, 1f);
    private Color happy = new Color(0.0392157f, 0.212f, 0.04367118f, 0.5f);
    private Color stress = new Color(0.2544924f, 0.03921569f, 0.282353f, 1f);
    private Color anxiety = new Color(0.07797226f, 0.0392157f, 0.1137255f, 1f);
    public Color neutral;
    private Color startColor;
    public Color endColor;

    //MESHES
    public string actualState;
    public string latestState;
    public string actualMesh;
    public MeshFilter meshFilter;
    private Mesh startMesh;
    private Mesh targetMesh;
    private Mesh morphedMesh;

    //PROGRESS
    private float transitionProgress = 0f;
    public float transitionDuration = 5f;

    //BOOLS
    public bool isMorphing = false;
    public bool alreadyChanged = false;
    public bool firstTime = true;
    private bool isTransitioning = false;

    //OTHERS
    public List<VegetationBehaviour> neighbourVegetation;
    private float radio;
    public string vegetationType;

    void Start()
    {
        playerFindsClosest = FindAnyObjectByType<PlayerFindsClosest>();
        if (playerFindsClosest == null) Debug.Log("PlayerFindsClosest Script not found");

        environmentController = FindAnyObjectByType<EnvironmentController>();
        if (environmentController == null) Debug.Log("EnvironmentController Script not found");

        vegetationRenderer = GetComponent<Renderer>();
        vegetationType = gameObject.tag;  // Make sure to tag the objects as "Tree" or "Plant" in Unity

        neutral = vegetationRenderer.material.color;

        if (vegetationType == "Tree")
        {
            startMesh = Resources.Load<Mesh>("Meshes/Trees/NeutralTree");
            targetMesh = Resources.Load<Mesh>("Meshes/Trees/NeutralTree");

            radio = 7f;
        }
        else if (vegetationType == "Plant")
        {
            startMesh = Resources.Load<Mesh>("Meshes/Plants/NeutralPlant");
            targetMesh = Resources.Load<Mesh>("Meshes/Plants/NeutralPlant");

            radio = 5f;
        }

        if (startMesh == null || targetMesh == null)
        {
            Debug.LogError("Meshes were not loaded correctly. Please check the path.");
            return;
        }

        morphedMesh = new Mesh();
        meshFilter.mesh = startMesh;
        morphedMesh.vertices = startMesh.vertices;
        morphedMesh.triangles = startMesh.triangles;
        morphedMesh.normals = startMesh.normals;
        morphedMesh.uv = startMesh.uv;

        neutral = vegetationRenderer.material.color;
        startColor = neutral;
        actualState = "neutral";
        latestState = actualState;

        if (vegetationType == "Tree")
            actualMesh = "NeutralTree";
        else if (vegetationType == "Plant")
            actualMesh = "NeutralPlant";

        neighbourVegetation = new List<VegetationBehaviour>();
        FindNeighbours();
    }

    void Update()
    {
        if (!environmentController.automaticMode)
        {
            HandleInput();

            if (isMorphing && !firstTime)
            {
                // Increment the interpolation progress
                transitionProgress += Time.deltaTime / transitionDuration;

                // Ensure the progress do not exceed 100%
                if (transitionProgress > 1f)
                {
                    transitionProgress = 1f;
                    startMesh = targetMesh;
                    isMorphing = false; // End morphing
                    alreadyChanged = true;
                    startColor = endColor;
                }

                // Interpolate the vertices between the two meshes
                MorphMeshes(startMesh, targetMesh, transitionProgress);
                vegetationRenderer.material.color = Color.Lerp(startColor, endColor, transitionProgress);
            }
        }

        else
        {
            firstTime = false;
            environmentController.ChangeState();

            if (isMorphing)
            {
                // Increment the interpolation progress
                transitionProgress += Time.deltaTime / transitionDuration;

                // Ensure the progress do not exceed 100%
                if (transitionProgress > 1f)
                {
                    transitionProgress = 1f;
                    startMesh = targetMesh;
                    isMorphing = false; // End morphing
                    alreadyChanged = true;
                    startColor = endColor;
                }

                // Interpolate the vertices between the two meshes
                MorphMeshes(startMesh, targetMesh, transitionProgress);
                vegetationRenderer.material.color = Color.Lerp(startColor, endColor, transitionProgress);
            }
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            actualState = "sad";
            if (vegetationType == "Plant")
                actualMesh = "SadPlant";
            else if(vegetationType == "Tree")
                actualMesh = "SadTree";
            else if (vegetationType == "Bush")
                actualMesh = "SadBush";

            alreadyChanged = false;
            firstTime = false;
            endColor = sad;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            actualState = "neutral";
            if (vegetationType == "Plant")
                actualMesh = "NeutralPlant";
            else if (vegetationType == "Tree")
                actualMesh = "NeutralTree";
            else if (vegetationType == "Bush")
                actualMesh = "NeutralBush";

            alreadyChanged = false;
            firstTime = false;
            endColor = neutral;
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            actualState = "happy";
            if (vegetationType == "Plant")
                actualMesh = "HappyPlant";
            else if (vegetationType == "Tree")
                actualMesh = "HappyTree";
            else if (vegetationType == "Bush")
                actualMesh = "HappyBush";

            alreadyChanged = false;
            firstTime = false;
            endColor = happy;
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            actualState = "angry";
            if (vegetationType == "Plant")
                actualMesh = "AngryPlant";
            else if (vegetationType == "Tree")
                actualMesh = "AngryTree";
            else if (vegetationType == "Bush")
                actualMesh = "AngryBush";

            alreadyChanged = false;
            firstTime = false;
            endColor = angry;
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            actualState = "anxiety";
            if (vegetationType == "Plant")
                actualMesh = "AnxiousPlant";
            else if (vegetationType == "Tree")
                actualMesh = "AnxiousTree";
            else if (vegetationType == "Bush")
                actualMesh = "AnxiousBush";

            alreadyChanged = false;
            firstTime = false;
            endColor = anxiety;
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            actualState = "stress";
            if (vegetationType == "Plant")
                actualMesh = "StressPlant";
            else if (vegetationType == "Tree")
                actualMesh = "StressTree";
            else if (vegetationType == "Bush")
                actualMesh = "StressBush";

            alreadyChanged = false;
            firstTime = false;
            endColor = stress;
        }
    }

    private void StartRandomMorphing()
    {
        isMorphing = true;
        MorphingProcess();
    }

    private void MorphingProcess()
    {
        transitionProgress += Time.deltaTime / transitionDuration;

        if (transitionProgress > 1f)
        {
            transitionProgress = 1f;
            startMesh = targetMesh;
            isMorphing = false;
        }

        MorphMeshes(startMesh, targetMesh, transitionProgress);
    }

    void MorphMeshes(Mesh fromMesh, Mesh toMesh, float t)
    {
        if (fromMesh.vertexCount != toMesh.vertexCount)
        {
            Debug.LogError("Meshes do not have the same number of vertices.");
            return;
        }

        Vector3[] fromVertices = fromMesh.vertices;
        Vector3[] toVertices = toMesh.vertices;
        Vector3[] morphedVertices = new Vector3[fromVertices.Length];

        for (int i = 0; i < fromVertices.Length; i++)
        {
            morphedVertices[i] = Vector3.Lerp(fromVertices[i], toVertices[i], t);
        }

        morphedMesh.vertices = morphedVertices;
        morphedMesh.triangles = fromMesh.triangles;
        morphedMesh.RecalculateNormals();
        meshFilter.mesh = morphedMesh;
    }

    private void FindNeighbours()
    {
        float distance;
        foreach (VegetationBehaviour vb in playerFindsClosest.allVegetation)
        {
            if(vb != this)
            {
                distance = Vector3.Distance(this.transform.position, vb.transform.position);
                if (distance < radio)
                {
                    neighbourVegetation.Add(vb); //Add closest vegetation to list
                }
            }
        }
    }

    public void MorphNeighbours(VegetationBehaviour vegetationMorphing)
    {
        alreadyChanged = false;
        
        if(latestState != actualState)
        {
            isMorphing = true;
            latestState = actualState;
        }

        if(!alreadyChanged)
            SetTargetMesh(vegetationMorphing.actualMesh, vegetationMorphing.actualState);

        if (neighbourVegetation.Count > 0)
        {
            StartCoroutine(OverlayMorphing());
        }
    }

    private IEnumerator OverlayMorphing()
    {
        yield return new WaitForSeconds(3f);

        // Initializes the interpolation of the neighbours
        foreach (VegetationBehaviour neighbour in neighbourVegetation)
        {

            if(!neighbour.alreadyChanged)
                neighbour.MorphNeighbours(neighbour);
        }
    }

    public void SetTargetMesh(string meshName, string state)
    {
        actualState = state;

        startMesh = meshFilter.mesh;
        string path = "";

        if (vegetationType == "Tree")
            path = "Meshes/Trees/";
        else if (vegetationType == "Plant")
            path = "Meshes/Plants/";
        else if (vegetationType == "Bush")
            path = "Meshes/Bushes/";

        targetMesh = Resources.Load<Mesh>($"{path}{meshName}");
        transitionProgress = 0f;
    }
}
