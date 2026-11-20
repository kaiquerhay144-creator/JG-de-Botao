using UnityEngine;

public class JGTouchController : MonoBehaviour
{
    public float maxForce = 9f;
    public float dragSensitivity = 0.018f;
    private Rigidbody selected;
    private Vector2 startPoint;
    private Vector2 currentPoint;
    private bool aiming;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = Input.mousePosition;
            currentPoint = startPoint;
            var ray = GetComponent<Camera>().ScreenPointToRay(startPoint);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.rigidbody != null && hit.collider.name != "Bola")
            {
                selected = hit.rigidbody;
                aiming = true;
            }
        }

        if (aiming && Input.GetMouseButton(0))
            currentPoint = Input.mousePosition;

        if (aiming && Input.GetMouseButtonUp(0))
        {
            Vector2 drag = startPoint - (Vector2)Input.mousePosition;
            Vector3 direction = new Vector3(drag.x, 0f, drag.y).normalized;
            float force = Mathf.Clamp(drag.magnitude * dragSensitivity, 0f, maxForce);
            if (selected != null && force > 0.15f)
                selected.AddForce(direction * force, ForceMode.Impulse);
            selected = null;
            aiming = false;
        }
    }

    private void OnGUI()
    {
        if (!aiming) return;
        Vector2 drag = startPoint - currentPoint;
        float force = Mathf.Clamp(drag.magnitude * dragSensitivity, 0f, maxForce);
        GUI.color = Color.white;
        GUI.Label(new Rect(18, Screen.height - 54, 320, 36), "Força do toque: " + force.ToString("0.0"));
    }
}
