using UnityEngine;

public class MagazineSpawner : MonoBehaviour
{
    [Header("生成するマガジンプレハブ")]
    public GameObject magazinePrefab;

    [Header("チェック対象のレイヤー")]
    public LayerMask magazineLayer;

    [Header("生成範囲")]
    public Vector3 boxSize = new Vector3(2f, 1f, 2f);

    [Header("生成位置（このGameObjectの位置が基準）")]
    public Vector3 spawnOffset = Vector3.zero;

    [Header("チェック間隔（秒）")]
    public float checkInterval = 1.0f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= checkInterval)
        {
            timer = 0f;

            // 範囲内のマガジンをチェック
            Vector3 center = transform.position + spawnOffset;
            Collider[] hitColliders = Physics.OverlapBox(center, boxSize / 2f, Quaternion.identity, magazineLayer);

            if (hitColliders.Length == 0)
            {
                // マガジンが無ければ新規生成
                Instantiate(magazinePrefab, center, Quaternion.identity);
                Debug.Log("マガジンを再生成しました");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + spawnOffset;
        Gizmos.DrawWireCube(center, boxSize);
    }
}