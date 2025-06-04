using UnityEngine;

public class TargetMover : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float approachDuration;
    private float sinkDuration;
    private float timer;
    private bool isMoving = true;

    public void Initialize( Vector3 endPos, float approachTime, float sinkTime)
    {
        startPosition = transform.position;
        targetPosition = endPos;
        approachDuration = Mathf.Max(0.01f, approachTime);
        sinkDuration = Mathf.Max(0.01f, sinkTime);
        timer = 0f;
        isMoving = true;

        transform.position = startPosition; // 確実にスタート位置に配置
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isMoving)
        {
            float t = timer / approachDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
 

            if (t >= 1.0f)
            {
                isMoving = false;
                timer = 0f;
            }
        }
        else
        {
            transform.position += Vector3.down * Time.deltaTime * 0.5f;

            if (timer >= sinkDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}
