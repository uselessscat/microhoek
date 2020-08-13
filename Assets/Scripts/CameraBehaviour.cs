using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public float panHorizontalMultiplier = 1f;
    public float panVerticalMultiplier = 1f;
    public float zoomMultiplier = 0.2f;
    public float moveMultiplier = 0.1f;

    private Vector3 center = new Vector3(0.5f, 0.5f, 0.0f);
    private Plane plane = new Plane(Vector3.up, Vector3.zero);

    private Camera camera;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    void Start() {
        camera = gameObject.GetComponent<Camera>();

        initialPosition = gameObject.transform.position;
        initialRotation = gameObject.transform.rotation;
    }

    void Update() {
        if (Input.touchCount == 1) {
            Ray centerRay = camera.ViewportPointToRay(center);

            float distance;

            if (plane.Raycast(centerRay, out distance)) {
                Vector3 intersectionPoint = centerRay.GetPoint(distance);
                Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;

                transform.RotateAround(intersectionPoint, Vector3.up, deltaPosition.x * panHorizontalMultiplier * Time.deltaTime);
                transform.RotateAround(intersectionPoint, transform.right, -deltaPosition.y * panVerticalMultiplier * Time.deltaTime);
            }
        }

        if (Input.touchCount == 2) {
            Touch firstTouch = Input.GetTouch(0), secondTouch = Input.GetTouch(1);

            // calcular zoom
            Vector2 firstPrevPosition = firstTouch.position - firstTouch.deltaPosition,
                secondPrevPosition = secondTouch.position - secondTouch.deltaPosition;

            float prevDeltaMagnitude = (firstPrevPosition - secondPrevPosition).magnitude,
                deltaMagnitude = (firstTouch.position - secondTouch.position).magnitude;

            float deltaMagnitudeDiff = (deltaMagnitude - prevDeltaMagnitude) * zoomMultiplier;

            // calcular deplazamiento
            Vector2 midpointPrevPosition = (firstPrevPosition + secondPrevPosition) / 2,
                midpointPosition = (firstTouch.position + secondTouch.position) / 2;

            Vector2 diffPosition = -(midpointPosition - midpointPrevPosition) * moveMultiplier;
            Vector3 diffPosition3d = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(diffPosition.x, 0, diffPosition.y);

            transform.position = camera.ViewportPointToRay(center).GetPoint(deltaMagnitudeDiff - camera.nearClipPlane) + diffPosition3d;
        }
    }

    public void onResetCamera() {
        gameObject.transform.position = initialPosition;
        gameObject.transform.rotation = initialRotation;
    }
}
