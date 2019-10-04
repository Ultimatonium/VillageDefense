using System.Collections.Generic;
using UnityEngine;

public abstract class DefenseTower : MonoBehaviour
{
    [SerializeField]
    private int cost;
    [SerializeField]
    private float shootDestroyTime;

    [SerializeField]
    protected int dmg;
    [SerializeField]
    protected float shootIntervall;
    [SerializeField]
    protected AudioClip shootSound;

    protected SimpleController controller;
    protected List<GameObject> enemies = new List<GameObject>();

    protected virtual void Start()
    {
        ToolBox.CheckSerializeField(ref cost);
        ToolBox.CheckSerializeField(ref shootDestroyTime);
        ToolBox.CheckSerializeField(ref dmg);
        ToolBox.CheckSerializeField(ref shootIntervall);
        ToolBox.CheckSerializeField(ref shootSound);
        controller = GameObject.Find("Main Camera").GetComponent<SimpleController>();
        InvokeRepeating("CleanupAudioSources", 1, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Remove(other.gameObject);
        }
    }

    protected void CleanupAudioSources()
    {
        foreach (AudioSource pew in this.gameObject.GetComponents<AudioSource>())
        {
            if (!pew.isPlaying)
            {
                Destroy(pew);
            }
        }
    }

    protected void PlayShotSound(AudioClip shotSoundClip, float volume)
    {
        AudioSource shotSound = this.gameObject.AddComponent<AudioSource>();
        shotSound.volume = volume;
        shotSound.priority = 256;
        shotSound.PlayOneShot(shotSoundClip);
    }

    protected void DrawShot(Vector3 target)
    {
        DrawShot(this.transform.position, target);
    }

    protected void DrawShot(Vector3 source, Vector3 target)
    {
        GameObject shoot = new GameObject();
        LineRenderer shootLineReder = shoot.AddComponent<LineRenderer>();
        shootLineReder.SetPosition(0, source);
        shootLineReder.SetPosition(1, target);
        shootLineReder.material = this.gameObject.GetComponent<Renderer>().material;
        Destroy(shoot, shootDestroyTime);
    }

    public void DisableVisualRange()
    {
        ToolBox.FindChildObject(ToolBox.FindChildObject(this.gameObject, "Scaler"), "TowerRange").GetComponent<MeshRenderer>().enabled = false;
    }

    public void EnableVisualRange()
    {
        ToolBox.FindChildObject(ToolBox.FindChildObject(this.gameObject, "Scaler"), "TowerRange").GetComponent<MeshRenderer>().enabled = true;
    }

    public List<GameObject> Enemies
    {
        get
        {
            return enemies;
        }
    }

    public int Cost
    {
        get
        {
            return cost;
        }
    }
}
