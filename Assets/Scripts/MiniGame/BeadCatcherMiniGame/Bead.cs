using UnityEngine;
using TMPro;

public class Bead : MonoBehaviour
{
    public char letter;
    public TextMeshProUGUI textDisplay;
    public float moveSpeed = 50f; // 천천히 이동

    public void SetLetter(char newLetter)
    {
        letter = newLetter;
        if (textDisplay != null) textDisplay.text = newLetter.ToString();
    }

    void Update()
    {
        // 왼쪽에서 오른쪽으로 이동 (UI 좌표계)
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);

        // 화면 밖으로 나가면 삭제 (X 좌표는 환경에 맞게 조정)
        if (transform.localPosition.x > 1000) Destroy(gameObject);
    }
}