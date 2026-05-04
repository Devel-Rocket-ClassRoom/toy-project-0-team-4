using UnityEngine;
using UnityEngine.UI;

public class PaddleController : MonoBehaviour
{
    public Slider uiSlider;
    private Rigidbody2D rb;

    [SerializeField] private float minX = -450f;
    [SerializeField] private float maxX = 455f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        uiSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        float targetX = Mathf.Lerp(minX, maxX, value);
        transform.localPosition = new Vector3(targetX, transform.localPosition.y, 0);
    }
}