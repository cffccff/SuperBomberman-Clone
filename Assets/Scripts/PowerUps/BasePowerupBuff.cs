using UnityEngine;
public class BasePowerupBuff : MonoBehaviour
{
    private Animator animator;
    private static readonly int Death = Animator.StringToHash("Death");
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Explosion"))
        {
            animator.SetTrigger(Death);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            ApplyBuffEffect(collision);
           
        }

    }
    //this function attached to the end of death animator stage
    private void DestroyPowerup()
    {
        Destroy(gameObject);
    }
    protected virtual void ApplyBuffEffect(Collider2D collision)
    {
        Destroy(gameObject);
        if (GameManager.instance == null) return;
        SFXManager.instance.PlayGetItemSound();
    }
}
