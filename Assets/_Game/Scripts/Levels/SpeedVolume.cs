using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpeedVolume : MonoBehaviour
{
    //Refer to Ben Friedman for QA/Bugfixing on SpeedVolume script

    private Collider triggerVolume = null;

    [Tooltip("Adjustment Made to PlayerShip ForwardSpeed")]
    [SerializeField] private SpeedVolumeSettings setting = SpeedVolumeSettings.Normal;

    private float fastDown = -1f;
    private float smallDown = -0.5f;
    private float smallUp = 0.5f;
    private float fastUp = 1f;

    private float normal = 5f;
    private float fullslow = 0f;
    private float fullthrottle = 10f;

    private void Awake()
    {
        triggerVolume = GetComponent<Collider>();
        triggerVolume.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Layers ensure only player can collide with SpeedVolume
        //player = other.GetComponent<PlayerController>();
        //AdjustPlayerSpeed(player);
    }

    /// <summary> Adjusts ForwardSpeed by amount specified in SpeedVolumeSetting
    /// <para>
    ///     Some settings are incremental boosts, speed += to setting
    ///     Some settings are firm numbers, set speed = to setting
    /// </para>
    /// </summary>
    /// <param name="playerReference"> PlayerBase reference passed from OnTriggerEnter collision </param>
    private void AdjustPlayerSpeed()    //parameter for Player, once playercontroller is established
    {
        Debug.Log("Player Speed is set to: ");
        switch (setting)
        {
            case SpeedVolumeSettings.FastDown:
                //break;
            case SpeedVolumeSettings.SmallDown:
                //break;
            case SpeedVolumeSettings.SmallUp:
                //break;
            case SpeedVolumeSettings.FastUp:
                //break;

            case SpeedVolumeSettings.Normal:
                //break;
            case SpeedVolumeSettings.FullSlow:
                //break;
            case SpeedVolumeSettings.FullThrottle:
                //break;

            default:
                Debug.Log("Speed Volume Entered");
                break;
        }
    }
}
