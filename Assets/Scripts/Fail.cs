using UnityEngine;


public class Fail : MonoBehaviour
{
    [Header("오디오 매니저")]
    [SerializeField] private AudioManager audioManager;

    private void OnEnable()
    {
        audioManager.PlaySfx(2);
    }
}