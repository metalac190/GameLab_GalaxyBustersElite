public enum BossState
{
    PreFight = -1,
    Idle = 0,
    Attack,
    Moving,
    Bloodied
}

public enum BossAttacks
{
    None = 0,
    MissileAttack = 1,
    RingAttack,
    LaserAttack,
    SummonMinions
}