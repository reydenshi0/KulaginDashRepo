using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform cameraTransform;

    [Range(0f, 1f)]
    [SerializeField] private float parallaxMultiplier = 0.5f;

    private Vector3 lastCameraPosition;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier, deltaMovement.y * parallaxMultiplier, 0);

        lastCameraPosition = cameraTransform.position;
    }
}
