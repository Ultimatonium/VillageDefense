using UnityEngine;
using UnityEngine.UI;

public class RepairWallScript : MonoBehaviour
{
    private Wall wall;
    private SimpleController controller;
    private Text text;

    private void Start()
    {
        wall = GameObject.Find("Wall").GetComponent<Wall>();
        controller = GameObject.Find("Main Camera").GetComponent<SimpleController>();
        text = ToolBox.FindChildObject(this.gameObject, "Text").GetComponent<Text>();
    }

    private void Update()
    {
        text.text = "Repair" + "\n" + "Cost: " + CalcRepairCost();
    }

    private int CalcRepairCost()
    {
        return (wall.maxHealth - wall.health) * 10;
    }

    public void RepairWall()
    {
        controller.money -= CalcRepairCost();
        wall.health = wall.maxHealth;
    }
}
