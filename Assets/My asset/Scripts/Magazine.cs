using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] int maxAmmo = 6;
    public int CurrentAmmo { get; private set; }

    void Awake()
    {
        CurrentAmmo = maxAmmo;
    }

    public bool TryUseAmmo()
    {
        if (CurrentAmmo <= 0) return false;

        CurrentAmmo--;
        return true;
    }

    public void ResetAmmo()
    {
        CurrentAmmo = maxAmmo;
    }
}
