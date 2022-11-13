using UnityEngine;

public class EnemyMoveAimless : EnemyMovement
{
  [SerializeField] private int timerToChooseDirection;
    [SerializeField] private float timerInFixUpdate;
    protected override void Start()
    {
        base.Start();
        timerToChooseDirection = Random.Range(3, 7);

    }
    protected override void FixedUpdate()
    {
        timerInFixUpdate += Time.fixedDeltaTime;

        if (timerInFixUpdate>= timerToChooseDirection)
        {
            direction = vector2FourDirection[Random.Range(0, 4)];
            timerInFixUpdate = 0;
            timerToChooseDirection = Random.Range(0, 5);
        }
        base.FixedUpdate();
    }
}
