using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFindsClosest : MonoBehaviour
{

    public Transform targetObject;

    public List<VegetationBehaviour> allVegetation;

    private VegetationBehaviour actualObject;

    // Start is called before the first frame update
    void Start()
    {
        allVegetation = new List<VegetationBehaviour>();
        FindVegetationObjects();
    }
    private void FindVegetationObjects()
    {
        VegetationBehaviour[] findOthers = FindObjectsOfType<VegetationBehaviour>();
        allVegetation.AddRange(findOthers);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Plant") || other.gameObject.CompareTag("Tree") || other.gameObject.CompareTag("Bush"))
        {
            actualObject = other.GetComponent<VegetationBehaviour>();

            if (!actualObject.isMorphing && actualObject.firstTime == false)
            {
                //Starts to morph & call MorphNeighbours function
                actualObject.MorphNeighbours(actualObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        actualObject = other.GetComponent<VegetationBehaviour>();
    }
}
