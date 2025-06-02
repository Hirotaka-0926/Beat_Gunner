using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using UnityEngine.XR.Content.Interaction;

public class MagazineSocket : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Gun to which the magazine will be attached")]
    GunProjectile gun = null;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        socket.selectEntered.AddListener(OnMagazineInserted);
    }

    void OnDestroy()
    {
        socket.selectEntered.RemoveListener(OnMagazineInserted);
    }

    void OnMagazineInserted(SelectEnterEventArgs args)
    {
        Magazine mag = args.interactableObject.transform.GetComponent<Magazine>();
        if (mag != null)
        {
            gun.SetMagazine(mag);
            Debug.Log("マガジン装填 完了");
            var rb = mag.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        }
    }

public void Eject()
{
    if (socket.hasSelection)
    {
        gun.EjectMagazine();

        var interactable = socket.firstInteractableSelected;
        if (interactable != null)
        {
            socket.interactionManager.SelectExit(socket, interactable);
        }

        Debug.Log("マガジン排出 完了");
    }
}
}