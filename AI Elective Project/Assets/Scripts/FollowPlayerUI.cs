using UnityEngine;

public class FollowPlayerUI : MonoBehaviour
{
    public Transform player; // Asigna aqu� el objeto del personaje
    public Vector3 offset; // Ajuste de la posici�n del texto sobre el personaje

    void Update()
    {
        if (player != null)
        {
            // Actualiza la posici�n del texto para que est� sobre el personaje
            transform.position = Camera.main.WorldToScreenPoint(player.position + offset);
        }
    }
}
