using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeactivateOnTrigger : MonoBehaviour
{
    public MonoBehaviour scriptToDeactivate;
    public string targetTag = "Player";

    private void Start()
    {
        if (scriptToDeactivate != null)
            scriptToDeactivate.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag) && scriptToDeactivate != null)
        {
            scriptToDeactivate.enabled = false;
        }
    }
}
