using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField]
    [Header("出現させる的プレハブ")]
    public GameObject targetPrefab;    // 出現させる的プレハブ

    [SerializeField]
    [Header("出現位置")]
    public Transform spawnPoint;            // 出現位置

    public void SpawnTarget()
    {
        Debug.Log("的を生成");
        GameObject target = Instantiate(targetPrefab, spawnPoint.position, Quaternion.identity);

        Destroy(target, 1f);

    }
    
    public void TestLog()
{
    Debug.Log("Signal successfully triggered!");
}
}
