using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    private void OnEnable()
    {
        Invoke(nameof(Destroy), 3f);
    }
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
    }
    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }
    private void Destroy()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }

}
