using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera exploreCamera;
    [SerializeField] private CinemachineCamera aimCamera;

    [SerializeField] private PlayerController playerController;

    void OnEnable()
    {
        playerController.OnStateUpdated += SwitchCamera;   
    }

    void OnDisable()
    {
        playerController.OnStateUpdated -= SwitchCamera;   
    }

    private void SwitchCamera(PlayerState state)
    {
        switch(state)
        {
            case PlayerState.EXPLORE:
                //DO SOMETHING
                exploreCamera.Prioritize();
                break;
            case PlayerState.AIM:
                //DO SOMETHING ELSE
                aimCamera.Prioritize();
                break;
            default:
                //NOTHING TO DO HERE
                break;
        }
    }
}