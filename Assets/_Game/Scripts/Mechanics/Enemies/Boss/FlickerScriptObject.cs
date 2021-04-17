using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/FlickerSettings")]
public class FlickerSettings : ScriptableObject
{
    /// A FlickerSettings Object is a ScriptableObject that contains data necessary to operate a FlickerController
    /// Like how a rigged game object needs an Animator with an AnimatorController reference
    /// 
    /// Certain enemie groups or object types should be able to share a single kind of settings, ie. the Boss + BossSegments share the BossFlickerSettings

    [Tooltip("Full cycle length of a single Flash\n(On and Off)")]
    public float FlickerTime = 0.1f;
    [Tooltip("Number of Flashes to take place per one damage")]
    public int FlickerNumber = 5;
    [Tooltip("The Material to alternate between")]
    public Material FlickerMaterial = null;
}
