using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassMorhper : MonoBehaviour
{
    public Terrain terrain; // Referencia al terreno
    public float transitionDuration = 2f; // Tiempo para completar la interpolaci�n
    private float transitionProgress = 0f;
    private bool isMorphing = false;

    private Color startColor = Color.green;
    private Color endColor = Color.red;
    private Color currentColor;
    private Material grassMaterial;

    private void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("Terreno no asignado.");
        }

        currentColor = startColor; // Inicializa el color actual

        //grassMaterial = terrain.terrainData.GetDetailPrototype(0).prototype as GameObject).GetComponent<Renderer>().material;

        // Establece el color inicial de la hierba
        UpdateGrassColor(currentColor);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) // Cambiar a un nuevo estado
        {
            StartMorphing();
        }

        if (isMorphing)
        {
            // Incrementar el progreso de la interpolaci�n
            transitionProgress += Time.deltaTime / transitionDuration;

            // Asegurarse de que el progreso no sobrepase 1
            if (transitionProgress > 1f)
            {
                transitionProgress = 1f;
                isMorphing = false;
            }

            // Interpolar el color de la hierba
            Color newColor = Color.Lerp(startColor, endColor, transitionProgress);
            UpdateGrassColor(newColor);
        }
    }

    private void StartMorphing()
    {
        // Cambia el color de inicio y fin para la morfolog�a
        startColor = currentColor;
        endColor = Random.ColorHSV(); // Cambiar a un color aleatorio o usa otros colores espec�ficos
        transitionProgress = 0f;
        isMorphing = true;
    }

    private void UpdateGrassColor(Color newColor)
    {
        // Obtener el �ndice del prototipo de detalle de hierba
        int detailIndex = 0; // Ajusta seg�n tu configuraci�n

        // Obtener la capa de detalles
        int[,] detailLayer = terrain.terrainData.GetDetailLayer(0, 0, terrain.terrainData.detailWidth, terrain.terrainData.detailHeight, detailIndex);

        // Iterar sobre el detalle layer y modificar el color
        for (int y = 0; y < terrain.terrainData.detailHeight; y++)
        {
            for (int x = 0; x < terrain.terrainData.detailWidth; x++)
            {
                if (detailLayer[x, y] > 0)
                {
                    // Aqu� puedes manipular el material de la hierba si es necesario
                    // Asignar color a la hierba
                    // Esto normalmente se har�a en el material del prefab de la hierba
                    // Puedes acceder al material de la hierba a trav�s del prefab utilizado en el prototipo de detalle
                }
            }
        }

        // Ejemplo de c�mo podr�as asignar el color a un material de hierba espec�fico (ajusta seg�n tu implementaci�n)
        // material de hierba en tu prefab
        // Material grassMaterial = ...; // Obt�n el material de la hierba del prefab
        // grassMaterial.color = newColor; // Cambia el color del material
    }
}
