
public class EnemyFinder : Finder<Enemy, Enemy>
{
    protected override Enemy GetTarget(Enemy enemy)
    {
        enemy.Died += OnEnemyDied;
        return enemy;
    }

    private void OnEnemyDied(Unit enemy)
    {
        enemy.Died -= OnEnemyDied;       
        Lose((Enemy)enemy);
    }
}
