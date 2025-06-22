
public abstract  class Enemy : Unit
{
    public override void TryTakeDamage(AttackData attackData)
    {
        TakeDamage(attackData);
    }
}
