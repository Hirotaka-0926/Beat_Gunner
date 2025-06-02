using UnityEngine;

public class TargetMover : MonoBehaviour
{
    private Vector3 targetPosition;
    private float approachDuration;
    private float sinkDuration;
    private float timer = 0f;

    private enum State { MovingToTarget, Dropping }
    private State currentState = State.MovingToTarget;

    public void Init(Vector3 finalPosition, float approachTime, float sinkTime)
    {
        targetPosition = finalPosition;
        approachDuration = Mathf.Max(0.01f, approachTime); // 0防止
        sinkDuration = Mathf.Max(0.01f, sinkTime);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (currentState == State.MovingToTarget)
        {
            float t = Mathf.Clamp01(timer / approachDuration);
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            if (t >= 1.0f)
            {
                currentState = State.Dropping;
                timer = 0f;
            }
        }
        else if (currentState == State.Dropping)
        {
            transform.position += Vector3.down * (Time.deltaTime * 0.5f);

            if (timer >= sinkDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}
