using UnityEngine;

namespace DM
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform player;
        public Vector3 offset;

        void Start()
        {
            player = GameObject.Find(GameObjectsInfo.playerName)?.GetComponent<Transform>();
        }

        void Update()
        {
            if (!player)
            {
                player = GameObject.Find(GameObjectsInfo.playerName)?.GetComponent<Transform>();
                return;
            }

            //get the players position and add it with offset, then store it to transform.position aka the cameras position
                transform.position = player.position + offset;
        }
    }
}
