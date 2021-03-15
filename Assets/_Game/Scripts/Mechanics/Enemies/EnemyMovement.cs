using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyZAxisMovement
    {
        CloseDistance,
        MaintainDistance
    }

    public enum EnemyMovementPatterns
    {
        Idle,
        North,
        South,
        East,
        West,
        Linear,
        Square,
        Circle,
        Bandit
    }

    public enum EnemySpawnMovement
    {
        SideArrival,
        BackArrival,
        AboveArrival
    }
}
