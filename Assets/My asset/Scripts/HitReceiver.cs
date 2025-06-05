using UnityEngine;

public class HitReceiverParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitParticlePrefab;
    [SerializeField] private float extraLifeTime = 0.3f;

    private bool _hitHandled = false;
    private const string BulletTag = "Bullet";

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(BulletTag) || _hitHandled) return;
        _hitHandled = true;

        // 衝突情報
        ContactPoint cp = collision.contacts[0];
        Vector3 pos     = cp.point;
        Quaternion rot  = Quaternion.LookRotation(cp.normal);

        // パーティクル生成
        if (hitParticlePrefab)
        {
            ParticleSystem ps = Instantiate(hitParticlePrefab, pos, rot);
            ps.Play();

            float ttl = ps.main.duration + ps.main.startLifetime.constantMax + extraLifeTime;
            Destroy(ps.gameObject, ttl);
        }

        // 弾を破棄
        Destroy(collision.gameObject);

        // ★ 自分（被弾オブジェクト）も破棄
        Destroy(gameObject);
    }
}