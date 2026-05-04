using UnityEngine;
using UnityEngine.UI;

public class PaddleController : MonoBehaviour
{
    public Slider uiSlider;
    private Rigidbody2D rb;

    [SerializeField] private float minX = -450f;
    [SerializeField] private float maxX = 455f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        uiSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        float targetX = Mathf.Lerp(minX, maxX, value);

        Vector2 targetPos = new Vector2(targetX, transform.localPosition.y);

        transform.localPosition = new Vector3(targetX, transform.localPosition.y, 0);
    }
}