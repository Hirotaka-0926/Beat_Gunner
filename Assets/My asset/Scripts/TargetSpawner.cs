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
 

        // 最終到達点
        Vector3 targetPosition = spawnPoint.position;

        // カメラ正面ベクトル（水平成分だけを使用）
        Vector3 viewDir = Camera.main.transform.forward;
        viewDir.y = 0f;          // 上下の傾きを無視
        viewDir.Normalize();

        // 奥方向 viewDir に spawnOffsetZ だけ離した所で出現
        Vector3 spawnPosition = targetPosition + viewDir * spawnOffsetZ;

        // プレハブ生成
        GameObject target = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);

        // TargetMover を追加して動作設定
        TargetMover mover = target.AddComponent<TargetMover>();
        mover.Initialize(targetPosition, approachTime, sinkTime);
    }
}
