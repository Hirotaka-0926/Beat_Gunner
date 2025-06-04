using UnityEngine;

public class HitReceiver : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;

    void OnCollisionEnter(Collision collision)
    {
        // ヒットした相手が弾か確認（タグなどで判定）
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // 命中位置と法線を取得
            ContactPoint contact = collision.contacts[0];
            Vector3 hitPosition = contact.point;
            Quaternion hitRotation = Quaternion.LookRotation(contact.normal);

            // エフェクトを生成
            if (hitEffectPrefab != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, hitPosition, hitRotation);
                Destroy(effect, 2f);
            }

            // 弾を破棄
            Destroy(collision.gameObject);
        }
    }
}