using System.Collections;
using UnityEngine;

namespace DM
{
    [RequireComponent(typeof(Collider2D))]
    public class ActivateOnTrigger : MonoBehaviour
    {
        public Animator anim;
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
                bool isWalking = anim.GetBool("IsWalking");
                if(isWalking)
                    return;

                var player = other.GetComponent<GridBasedMovement>();
                if (player != null)
                    player.canWalk = false;

                StartCoroutine(WakeThenEnableMovement(player));
            }
        }

        private IEnumerator WakeThenEnableMovement(GridBasedMovement player)
        {
            anim.SetTrigger("WakeUp");
            anim.SetBool("IsWalking", true);

            // wait until the wakeup animation is finished
            while (true)
            {
                var s = anim.GetCurrentAnimatorStateInfo(0);
                if (s.IsName("WakeUp") && s.normalizedTime >= .90f && player != null)
                    player.canWalk = true;
                if (s.IsName("WakeUp") && s.normalizedTime >= .98f)
                        break;
                yield return null;
            }

            scriptToActivate.enabled = true;
        }
    }
}
