using UnityEngine;

public class DmgAllEnemyInRange : DefenseTower
{
    protected override void Start()
    {
        base.Start();
        controller.DefenseObjects.Add(this);
        InvokeRepeating("HitAllEnemies", 0, shootIntervall);
    }

    private void HitAllEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            try
            {
                DrawShot(enemy.transform.position);
                PlayShotSound(shootSound, 0.1f);
                enemy.GetComponent<SimpleEnemy>().ApplyDamage(dmg);
            }
            catch (MissingReferenceException e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }
}
