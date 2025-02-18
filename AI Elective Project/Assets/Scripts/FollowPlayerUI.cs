using UnityEngine;

public class FollowPlayerUI : MonoBehaviour
{
    public Transform player; // Asigna aquí el objeto del personaje
    public Vector3 offset; // Ajuste de la posición del texto sobre el personaje

    void Update()
    {
        if (player != null)
        {
            // Actualiza la posición del texto para que esté sobre el personaje
            transform.position = Camera.main.WorldToScreenPoint(player.position + offset);
        }
    }
}
