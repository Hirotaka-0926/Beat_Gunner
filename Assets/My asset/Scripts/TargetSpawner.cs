using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("出現させる的プレハブ")]
    [SerializeField] private GameObject targetPrefab;

    [Header("出現位置")]
    [SerializeField] private Transform spawnPoint;

    [Header("出現位置の奥行きオフセット")]
    [SerializeField] private float spawnOffsetZ = 2f;

    [Header("到達までの時間（秒）")]
    [SerializeField] private float approachTime = 1.0f;

    [Header("到達後に沈む時間（秒）")]
    [SerializeField] private float sinkTime = 1.0f;

    public void SpawnTarget()
    {
        Debug.Log("的を生成");

        // 出現位置を spawnPoint のZ方向奥に設定
        Vector3 spawnPosition = spawnPoint.position + spawnPoint.forward * spawnOffsetZ;
        Vector3 targetPosition = spawnPoint.position;

        // プレハブ生成
        GameObject target = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);

        // TargetMover を追加して動作設定
        TargetMover mover = target.AddComponent<TargetMover>();
        mover.Initialize(targetPosition, approachTime, sinkTime);
    }
}
