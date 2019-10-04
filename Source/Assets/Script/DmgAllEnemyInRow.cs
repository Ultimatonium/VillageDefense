using System;
using System.Collections;
using UnityEngine;

public class DmgAllEnemyInRow : DefenseTower
{
    [SerializeField]
    private LayerMask enemyMask; //todo in start

    private RaycastHit[] hits;

    protected override void Start()
    {
        ToolBox.CheckSerializeField(ref enemyMask);
        base.Start();
        controller.DefenseObjects.Add(this);
        InvokeRepeating("HitAllEnemiesInRow", 0, shootIntervall);
    }

    private void HitAllEnemiesInRow()
    {
        if (enemies.Count > 0)
        {
            StartCoroutine(DoRail());
        }
    }
    
    private IEnumerator DoRail()
    {
        // InitSound
        AudioSource audio = this.gameObject.AddComponent<AudioSource>();
        audio.clip = shootSound;
        audio.volume = 0.7f;
        audio.priority = 256;

        // Loading Sound
        audio.time = 0;
        audio.Play();
        audio.SetScheduledEndTime(AudioSettings.dspTime + (0.8f - 0));
        yield return new WaitForSeconds(0.8f);

        if (enemies.Count > 0)
        {
            // Shooting Sound
            audio.time = 0.8f;
            audio.Play();

            // Rest
            try
            {
                GameObject target = enemies[0];
                Vector3 heading = target.transform.position - this.transform.position;
                Vector3 direction = heading / heading.magnitude;
                direction = new Vector3(direction.x, 0, direction.z);
                hits = Physics.RaycastAll(this.transform.position, direction, Mathf.Infinity, enemyMask);
                base.DrawShot(new Vector3(this.transform.position.x, 1, this.transform.position.z), this.transform.position + new Vector3(direction.x * 100, 1, direction.z * 100));
                foreach (RaycastHit enemy in hits)
                {
                    enemy.collider.gameObject.GetComponent<SimpleEnemy>().ApplyDamage(dmg);
                }
            }
            catch (MissingReferenceException e)
            {
                Debug.LogWarning(e.Message);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }
}
