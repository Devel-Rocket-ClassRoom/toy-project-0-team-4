using UnityEngine;

public class TrapdoorController : MonoBehaviour
{
    public RectTransform leftDoor;
    public RectTransform rightDoor;

    public float openSpeed = 8f; // ���� ������ �ӵ� (�������� ����)

    private float openAngle = 90f;
    private bool isOpen = false;
    private float targetAngle = 0f;
    private float currentAngle = 0f;

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        targetAngle = isOpen ? openAngle : 0f;
    }

    void Update()
    {
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * openSpeed);
        leftDoor.localRotation = Quaternion.Euler(0, 0, -currentAngle);
        rightDoor.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
