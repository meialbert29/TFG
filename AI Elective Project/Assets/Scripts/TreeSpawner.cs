using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{

    private PlayerFindsClosest playerFindsClosest;

    public VegetationBehaviour treePrefab;
    public VegetationBehaviour plantPrefab;

    public int treeCount = 100; // Number of trees you want to generate
    public int plantCount = 10;
    public float terrainWidth = 100f; // Terrain width
    public float terrainLength = 100f; // Terrain length
    public float minDistanceBetweenTrees = 5f; // Minimum distance between trees
    public float minDistanceBetweenPlants = 3f;

    private List<Vector3> treePositions = new List<Vector3>();
    private List<Vector3> plantPositions = new List<Vector3>();

    void Start()
    {

        playerFindsClosest = FindAnyObjectByType<PlayerFindsClosest>();
        if (playerFindsClosest == null) Debug.Log("PlayerFindsClosest Script not found");

        SpawnTrees();
        SpawnPlants();
    }

    void SpawnTrees()
    {
        for (int i = 0; i < treeCount; i++)
        {
            Vector3 position;
            int attempts = 0;
            bool validPosition = false;

            // Try several times to find a valid position
            do
            {
                // Generate a random position on the terrain
                float xPos = Random.Range(0, terrainWidth);
                float zPos = Random.Range(0, terrainLength);
                position = new Vector3(xPos, 0, zPos);

                // Check if the position is far enough from existing trees
                validPosition = true;
                foreach (Vector3 existingPosition in treePositions)
                {
                    if (Vector3.Distance(position, existingPosition) < minDistanceBetweenTrees)
                    {
                        validPosition = false;
                        break;
                    }
                }

                attempts++;
            }
            while (!validPosition && attempts < 10); // Limit attempts to avoid infinite loops

            if (validPosition)
            {
                treePositions.Add(position);
                Quaternion treeRotation = Quaternion.Euler(-90, 0, 0); // Rotation on the X axis
                VegetationBehaviour treeInstantiate = Instantiate(treePrefab, position, treeRotation);

                playerFindsClosest.allVegetation.Add(treeInstantiate);
            }
        }
    }

    void SpawnPlants()
    {
        for (int i = 0; i < plantCount; i++)
        {
            Vector3 position;
            int attempts = 0;
            bool validPosition = false;

            // Try several times to find a valid position
            do
            {
                // Generate a random position on the terrain
                float xPos = Random.Range(0, terrainWidth);
                float zPos = Random.Range(0, terrainLength);
                position = new Vector3(xPos, 0, zPos);

                // Check if the position is far enough from existing plants
                validPosition = true;
                foreach (Vector3 existingPosition in plantPositions)
                {
                    if (Vector3.Distance(position, existingPosition) < minDistanceBetweenPlants)
                    {
                        validPosition = false;
                        break;
                    }
                }

                attempts++;
            }
            while (!validPosition && attempts < 10); // Limit attempts to avoid infinite loops

            if (validPosition)
            {
                plantPositions.Add(position);
                Quaternion plantRotation = Quaternion.Euler(-90, 0, 0); // Rotation on the X axis
                VegetationBehaviour plantInstantiate = Instantiate(plantPrefab, position, plantRotation);

                playerFindsClosest.allVegetation.Add(plantInstantiate);
            }
        }
    }
}
