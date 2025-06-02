namespace UnityEngine.XR.Content.Interaction
{
    /// <summary>
    /// Apply forward force to instantiated prefab
    /// </summary>
    public class GunProjectile : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The projectile that's created")]
        GameObject m_ProjectilePrefab = null;

        [SerializeField]
        [Tooltip("The point that the project is created")]
        Transform m_StartPoint = null;

        [SerializeField]
        [Tooltip("The speed at which the projectile is launched")]
        float m_LaunchSpeed = 1.0f;


        [Header("Ammo Settings")]

        [SerializeField]
        [Tooltip("弾切れ時のサウンド")]
        AudioClip m_OutOfAmmoSound = null;

        [SerializeField]
        [Tooltip("弾を撃つときのAudioSource")]
        AudioSource m_FireAudioSource = null;

        private Magazine currentMagazine;

        public void Fire()
        {
            if (currentMagazine == null || !currentMagazine.TryUseAmmo() )
            {
                if (m_OutOfAmmoSound != null)
                {
                    AudioSource.PlayClipAtPoint(m_OutOfAmmoSound, m_StartPoint.position);
                }

                return;
            }
            GameObject newObject = Instantiate(m_ProjectilePrefab, m_StartPoint.position, m_StartPoint.rotation, null);


            if (newObject.TryGetComponent(out Rigidbody rigidBody)) {
                ApplyForce(rigidBody);
            }


            if (m_FireAudioSource != null)
            {
                m_FireAudioSource.Play();
            }

        }

        public void SetMagazine(Magazine mag)
        {
            currentMagazine = mag;
        }

        public void EjectMagazine()
        {
            currentMagazine = null;
        }

        void ApplyForce(Rigidbody rigidBody)
        {
            Vector3 force = m_StartPoint.forward * m_LaunchSpeed;
            rigidBody.AddForce(force);
        }
    }
}
