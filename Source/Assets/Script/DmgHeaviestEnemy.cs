using UnityEngine;

public class DmgHeaviestEnemy : DefenseTower
{
    protected override void Start()
    {
        base.Start();
        controller.DefenseObjects.Add(this);
        InvokeRepeating("HitHeaviestEnemy", 0, shootIntervall);
    }

    private void HitHeaviestEnemy()
    {
        if (enemies.Count > 0)
        {
            try
            {
                GameObject heaviestEnemy = enemies[0];
                foreach (GameObject enemy in enemies)
                {
                    if (enemy.GetComponent<SimpleEnemy>().value > heaviestEnemy.GetComponent<SimpleEnemy>().value)
                    {
                        heaviestEnemy = enemy;
                    }
                }
                DrawShot(heaviestEnemy.transform.position);
                PlayShotSound(shootSound, 0.5f);
                heaviestEnemy.GetComponent<SimpleEnemy>().ApplyDamage(dmg);
            }
            catch (MissingReferenceException e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

}
