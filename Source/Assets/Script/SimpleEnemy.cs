using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SimpleEnemy : MonoBehaviour
{
    [SerializeField]
    private int initialHealth;
    [SerializeField]
    private int initialDmg;

    [HideInInspector]
    public int dmg;
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public int value;

    private int health;
    private TextMesh healthText;
    private TextMesh dmgText;
    private TextMesh valueText;
    private Wall wall;
    private SimpleController controller;
    private NavMeshAgent agent;
    
    private void Start()
    {
        ToolBox.CheckSerializeField(ref initialHealth);
        ToolBox.CheckSerializeField(ref initialDmg);
        health = maxHealth;
        healthText = ToolBox.FindChildObject(this.gameObject, "EnemyHealth").GetComponent<TextMesh>();
        dmgText = ToolBox.FindChildObject(this.gameObject, "EnemyDmg").GetComponent<TextMesh>();
        valueText = ToolBox.FindChildObject(this.gameObject, "EnemyValue").GetComponent<TextMesh>();
        controller = GameObject.Find("Main Camera").GetComponent<SimpleController>();
        wall = GameObject.Find("Wall").GetComponent<Wall>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = new Vector3(wall.transform.position.x, 0, this.transform.position.z);
    }

    private void Update()
    {
        SetText(healthText, "Health: " + health + " / " + maxHealth);
        SetText(dmgText, "Dmg: " + dmg);
        SetText(valueText, "Value: " + value);
    }

    private void SetText(TextMesh enemyText, string text)
    {
        enemyText.text = text;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            wall.ApplyDamage(dmg);
        }
    }

    public void ApplyDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Destroy(this.gameObject, 0.1f);
        }
    }

    private void OnDestroy()
    {
        controller.EnemyKilled(this.gameObject);
    }
    
    public int InitialHealth
    {
        get
        {
            return initialHealth;
        }
    }

    public int InitialDmg
    {
        get
        {
            return initialDmg;
        }
    }
}
