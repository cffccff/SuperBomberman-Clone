using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float explosionDuration;
    public GameObject belongToBomb;
    private void OnEnable()
    {
        Invoke(nameof(SetDisable), explosionDuration);
    }
    private void SetDisable()
    {
         gameObject.SetActive(false);
    }
}

