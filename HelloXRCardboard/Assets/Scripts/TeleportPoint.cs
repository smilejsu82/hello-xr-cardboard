using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportPoint : MonoBehaviour
{
    public UnityEvent onTeleportEnter;
    public UnityEvent onTeleport;
    public UnityEvent onTeleportExit;

    private void Start()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnPointerEnterXR() {
        this.onTeleportEnter?.Invoke();
    }

    public void OnPointerClickXR()
    {
        this.ExecuteTeleportation();
        this.onTeleport?.Invoke();
        TeleportManager.instance.DisableTeleportPoiint(this.gameObject);
    }

    public void OnPointerExitXR()
    {
        this.onTeleportExit?.Invoke();
    }

    private void ExecuteTeleportation() {
        var player = TeleportManager.instance.player;
        player.transform.position = this.transform.position;
        Camera cam = player.GetComponentInChildren<Camera>();
        var rotY = this.transform.rotation.eulerAngles.y - cam.transform.localEulerAngles.y;
        player.transform.rotation = Quaternion.Euler(0, rotY, 0);
    }
}
