using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAim : MonoBehaviour
{
    private Camera mainCamera;
    public Vector2 AimDirection { get; private set; }
    private void Awake()
    {
        mainCamera = Camera.main;

    }

    private void Update()
    {
        if (Mouse.current == null) return;
        Vector3 mouseWorldPosition = Mouse.current.position.ReadValue();
        mouseWorldPosition.z = 10f;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseWorldPosition);
        AimDirection = (worldPosition - transform.position).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)AimDirection * 1f);
    }
}
