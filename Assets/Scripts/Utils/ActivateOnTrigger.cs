using UnityEngine;

namespace DM
{
    [RequireComponent(typeof(Collider2D))]
    public class ActivateOnTrigger : MonoBehaviour
    {
        public MonoBehaviour scriptToActivate;
        public string targetTag = "Player";

        private void Start()
        {
            if (scriptToActivate != null)
                scriptToActivate.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(targetTag) && scriptToActivate != null)
            {
                scriptToActivate.enabled = true;
            }
        }
    }
}
