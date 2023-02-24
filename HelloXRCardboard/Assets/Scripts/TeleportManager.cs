using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager instance;
    public GameObject player;
    private GameObject lastTeleportPoint;

    private void Awake()
    {
        if (instance != this && instance != null)
            Destroy(this);
        else
            instance = this;
    }

    public void DisableTeleportPoiint(GameObject teleportPoint)
    {
        if (this.lastTeleportPoint != null)
            this.lastTeleportPoint.SetActive(true);

        teleportPoint.SetActive(false);
        this.lastTeleportPoint = teleportPoint;
#if UNITY_EDITOR
        this.player.GetComponent<CardboardSimulator>().UpdatePlayerPositonSimulator();
#endif
    }
}
