using UnityEngine;
using UnityEngine.UI;

public class ResetCamera : MonoBehaviour
{
    public CameraBehaviour camera;

    void Start() {
        gameObject.GetComponent<Button>().onClick.AddListener(camera.onResetCamera);
    }

    void Update() {

    }
}
