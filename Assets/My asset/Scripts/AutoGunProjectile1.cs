namespace UnityEngine.XR.Content.Interaction
{
    /// <summary>
    /// Apply forward force to instantiated prefab
    /// </summary>
    public class AutoGunProjectile : GunProjectile
    {
public float interval = 0.001f; // 実行間隔（秒）
    private float timer = 0f;

        private bool isTriggered = false;


        public void TriggerFire()
        {
            isTriggered = true;
            InvokeRepeating("Fire", 0f, 0.1f);
        }

        public void StopFire()
        {
            isTriggered = false;
            CancelInvoke();
        }
    }
}
