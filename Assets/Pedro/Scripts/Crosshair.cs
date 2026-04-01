using System;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Canvas crosshairCanvas;

    private void OnEnable()
    {
        player.OnStateUpdated += StateUpdate;
    }

    private void OnDisable()
    {
        player.OnStateUpdated -= StateUpdate;
    }

    private void Start()
    {
        crosshairCanvas.enabled = false;
    }

    void StateUpdate(PlayerState state)
    {
        crosshairCanvas.enabled = state == PlayerState.AIM;
    }
}
