using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDrone : EnemyBase
{
    protected override void Passive()
    {
        transform.LookAt(GameManager.player.obj.transform.position);
    }
    
    protected override void Attacking()
    { }
}
