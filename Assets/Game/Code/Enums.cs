namespace Game.Code
{
    public enum Round
    {
        Round1,
        Round2,
        Round3
    }

    public enum CardType
    {
        Attack3,
        Attack6,
        Attack1014,
        Defence5,
        Defence8,
        Defence12,
        Buff1,
        Buff2,
        Buff3,
        Debuff
    }

    public enum EnemyType
    {
        First,
        Second,
        Third
    }

    public enum DebuffType
    {
        None,
        ReduceDamage1,
        ReduceDamage2Shield,
        HealOnAttack
    }
}