using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerLanes player;
    [Tooltip("How far ahead of the player the camera centers while running. Higher = player sits further back on screen")]
    [SerializeField] private float lookAhead = 4f;
    [Tooltip("Extra camera rotation so the path runs diagonally across the screen. 0 = straight up, 60 = up-right like the original")]
    [SerializeField] private float viewAngle = 60f;
    [Tooltip("How fast the camera rotates when the path turns (degrees per second)")]
    [SerializeField] private float turnSpeed = 180f;

    [Header("At Junctions")]
    [Tooltip("Look-ahead while stopped at a junction. 0 = camera centers on the junction so both side paths are visible")]
    [SerializeField] private float junctionLookAhead = 0f;
    [Tooltip("Camera size while stopped at a junction. Bigger = zoomed out more")]
    [SerializeField] private float junctionZoom = 6f;
    [Tooltip("How quickly the camera slides and zooms between running and junction views")]
    [SerializeField] private float transitionSpeed = 4f;

    // The extra diagonal rotation (PathGenerator uses this to place kitchen props)
    public float ViewAngle => viewAngle;

    private Camera cam;
    private float runningZoom;
    private float currentLookAhead;

    // Direction the camera looks ahead in. Turns smoothly toward the player's heading.
    private Vector3 lookDirection;

    private void Start()
    {
        cam = GetComponent<Camera>();
        runningZoom = cam.orthographicSize;
        currentLookAhead = lookAhead;
        lookDirection = player.Heading;

        // Start already at the right angle instead of turning into it
        transform.rotation = player.HeadingRotation * Quaternion.Euler(0f, 0f, viewAngle);
    }

    private void LateUpdate()
    {
        // Blend between the running view and the junction view
        float blend = 1f - Mathf.Exp(-transitionSpeed * Time.deltaTime);
        bool atJunction = !player.IsRunning;

        currentLookAhead = Mathf.Lerp(currentLookAhead, atJunction ? junctionLookAhead : lookAhead, blend);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, atJunction ? junctionZoom : runningZoom, blend);

        lookDirection = Vector3.RotateTowards(
            lookDirection,
            player.Heading,
            turnSpeed * Mathf.Deg2Rad * Time.deltaTime,
            0f);

        Vector3 target = player.PathCenter + lookDirection * currentLookAhead;
        target.z = transform.position.z;
        transform.position = target;

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            player.HeadingRotation * Quaternion.Euler(0f, 0f, viewAngle),
            turnSpeed * Time.deltaTime);
    }
}