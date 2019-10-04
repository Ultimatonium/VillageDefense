using UnityEngine;
using UnityEngine.UI;

public class CreateTower : MonoBehaviour
{
    [SerializeField]
    private GameObject tower;

    private SimpleController controller;

    private void Start()
    {
        ToolBox.CheckSerializeField(ref tower);
        controller = GameObject.Find("Main Camera").GetComponent<SimpleController>();
        SetButtonCostText(this.gameObject, tower);
    }

    public static void SetButtonCostText(GameObject button, GameObject tower)
    {
        ToolBox.FindChildObject(button, "Text").GetComponent<Text>().text = tower.name + "\n" + "Cost: " + tower.GetComponent<DefenseTower>().Cost;
    }

    public void InitTower()
    {
        if (controller.objectAttachedOnMouse == null)
        {
            if (controller.money >= tower.GetComponent<DefenseTower>().Cost)
            {
                GameObject newTower = Instantiate(tower, new Vector3(0, -100, 0), Quaternion.identity);
                controller.MakeTowerInnactive(newTower);
                controller.objectAttachedOnMouse = newTower;
            }
            else
            {
                controller.SetStatusText("Nicht genug Geld");
            }
        }
    }
}
