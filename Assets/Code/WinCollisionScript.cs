using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollisionScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HammyGameStateManager.Get().Win();
        }
    }
}
