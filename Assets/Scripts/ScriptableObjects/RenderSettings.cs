using UnityEngine;


namespace DM
{
    [CreateAssetMenu(fileName = "RenderSettings", menuName = "Render Settings")]
    public class RenderSettings : ScriptableObject
    {
        [Header("Camera")]
        public float zoomSpeed = 0f;

        [Space()]
        public float defaultZoom = 1.8f;

        [Space()]
        public float minZoom = 1f;
        public float maxZoom = 3f;
    }
}
