using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSwitch : MonoBehaviour
{
    public List<GameObject> objectsToActivate = new List<GameObject>(); 
    public List<GameObject> objectsToDeactivate = new List<GameObject>(); 
    public bool destroyAfterTrigger = true; 
    public string tagTriggerWith = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(tagTriggerWith)) 
        {    
            return;
        }
        
        ActivateObjects();
        DeactivateObjects();

        if (destroyAfterTrigger)
        {
            Destroy(gameObject); 
        }
    }

    private void ActivateObjects()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    private void DeactivateObjects()
    {
        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
