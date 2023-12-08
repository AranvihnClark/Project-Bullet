using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBoundary : MonoBehaviour
{
    // Don't know if variables are needed yet
    
    private void Update()
    {
        // Tick the player death timer
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Start tracking time (2 or 3 seconds) before player is automatically 'defeated'
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Resets player death timer
    }
}
