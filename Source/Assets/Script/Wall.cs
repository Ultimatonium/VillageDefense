using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    public int maxHealth;

    [HideInInspector]
    public int health;

    private TextMesh wallHealthText;

    private void Start()
    {
        ToolBox.CheckSerializeField(ref maxHealth);
        health = maxHealth;
        wallHealthText = ToolBox.FindChildObject(ToolBox.FindChildObject(this.gameObject, "Scaler"), "WallHealthText").GetComponent<TextMesh>();
    }

    private void Update()
    {
        wallHealthText.text = health.ToString() + "/" + maxHealth.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject, 0.1f);
        }
    }

    public void ApplyDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Debug.Log("game over");
            LoadScene.SimpleSyncSceneLoading("GameOverScene");
        }
    }
}
