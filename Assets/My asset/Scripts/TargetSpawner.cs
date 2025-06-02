using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField]
    [Header("出現させる的プレハブ")]
    public GameObject targetPrefab;

    [SerializeField]
    [Header("出現位置")]
    public Transform spawnPoint;

    [SerializeField]
    [Header("出現位置の奥行きオフセット")]
    public float spawnOffsetZ = 2f;

    [SerializeField]
    [Header("到達までの時間（秒）")]
    public float approachTime = 0.3f;

    [SerializeField]
    [Header("到達後に沈む時間（秒）")]
    public float sinkTime = 0.2f;

    public void SpawnTarget()
    {
        Debug.Log("的を生成");

        Vector3 spawnPosition = spawnPoint.position + new Vector3(0, 0, spawnOffsetZ);
        GameObject target = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);

        TargetMover mover = target.AddComponent<TargetMover>();
        mover.Init(spawnPoint.position, approachTime, sinkTime);
    }
}

