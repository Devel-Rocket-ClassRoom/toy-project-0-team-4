using UnityEngine;


public class Succes : MonoBehaviour
{
    [Header("오디오 매니저")]
    [SerializeField] private AudioManager audioManager;

    private void OnEnable()
    {
        audioManager.PlaySfx(4);
    }
}