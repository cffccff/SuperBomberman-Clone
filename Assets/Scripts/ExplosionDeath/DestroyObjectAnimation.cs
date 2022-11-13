using UnityEngine;

public class DestroyObjectAnimation : MonoBehaviour
{

    private void OnEnable()
    {
        Invoke(nameof(DisableObject),1f);
    }
    private void DisableObject()
    {
       
        gameObject.SetActive(false);
    }
}
