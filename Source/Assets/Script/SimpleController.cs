using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SimpleController : MonoBehaviour
{
    [SerializeField]
    private int startMoney;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private LayerMask groundMask; //todo in start
    [SerializeField]
    private LayerMask towerbodyMask; //todo in start

    [HideInInspector]
    public GameObject objectAttachedOnMouse;
    [HideInInspector]
    public int money;

    private GameObject wall;
    private GameObject ui;
    private NavMeshAgent pathfinder;
    private NavMeshPath path;
    private GameObject moneyText;
    private GameObject enemyCounter;
    private GameObject lastPlacedTower;
    private int killedEnemy;
    private int createdEnemy;
    private RaycastHit hit;
    private List<DefenseTower> defenseObjects;

    private void Start()
    {
        ToolBox.CheckSerializeField(ref startMoney);
        ToolBox.CheckSerializeField(ref enemyPrefab);
        ToolBox.CheckSerializeField(ref groundMask);
        ToolBox.CheckSerializeField(ref towerbodyMask);

        objectAttachedOnMouse = null;
        path = new NavMeshPath();
        defenseObjects = new List<DefenseTower>();

        money = startMoney;

        killedEnemy = 0;
        createdEnemy = 0;

        pathfinder = GameObject.Find("Pathfinder").GetComponent<NavMeshAgent>();
        ui = GameObject.Find("Canvas");
        wall = GameObject.Find("Wall");
        InvokeRepeating("SpawnRandomEnemy", 1, 2f);
        moneyText = ToolBox.FindChildObject(ui, "MoneyText");
        enemyCounter = ToolBox.FindChildObject(ui, "EnemyCounter");
    }


    private void Update()
    {
        moneyText.GetComponent<Text>().text = "Money: " + money.ToString();
        SetEnemyCounter();

        Ray mp = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(objectAttachedOnMouse);
            objectAttachedOnMouse = null;
            DesableAllVisualRanges();
        }

        SelectPlacedTower(mp);
        PlaceNewTower(mp);
        CheckIfPathBlocked();
    }

    private void SelectPlacedTower(Ray position)
    {
        if (Input.GetMouseButtonDown(0) && objectAttachedOnMouse == null)
        {
            if (Physics.Raycast(position, out hit, 150, towerbodyMask))
            {
                DesableAllVisualRanges();
                hit.collider.gameObject.GetComponent<DefenseTower>().EnableVisualRange();
            }
            else
            {
                DesableAllVisualRanges();
            }
        }
    }

    private void PlaceNewTower(Ray position)
    {
        if (objectAttachedOnMouse != null)
        {
            if (Physics.Raycast(position, out hit, 150, groundMask))
            {
                objectAttachedOnMouse.transform.position = new Vector3(hit.point.x, 4, hit.point.z);

                if (Input.GetMouseButtonDown(0))
                {
                    if (CheckPositionIsFree(position.origin, objectAttachedOnMouse))
                    {
                        MakeTowerActive(objectAttachedOnMouse);
                        lastPlacedTower = objectAttachedOnMouse;
                        money -= objectAttachedOnMouse.GetComponent<DefenseTower>().Cost;
                        objectAttachedOnMouse = null;
                    }
                    else
                    {
                        SetStatusText("Tower können nicht aufeinander gebaut werden");
                    }
                }
            }
        }
    }

    private bool CheckPositionIsFree(Vector3 position, GameObject newObject)
    {
        if (Physics.Raycast(new Vector3(position.x - newObject.transform.lossyScale.x / 2, position.y, position.z + newObject.transform.lossyScale.z / 2), Vector3.down, out hit, 150, towerbodyMask)
         || Physics.Raycast(new Vector3(position.x + newObject.transform.lossyScale.x / 2, position.y, position.z + newObject.transform.lossyScale.z / 2), Vector3.down, out hit, 150, towerbodyMask)
         || Physics.Raycast(new Vector3(position.x - newObject.transform.lossyScale.x / 2, position.y, position.z - newObject.transform.lossyScale.z / 2), Vector3.down, out hit, 150, towerbodyMask)
         || Physics.Raycast(new Vector3(position.x + newObject.transform.lossyScale.x / 2, position.y, position.z - newObject.transform.lossyScale.z / 2), Vector3.down, out hit, 150, towerbodyMask)
            )
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void DesableAllVisualRanges()
    {
        foreach (DefenseTower defenseObject in defenseObjects)
        {
            defenseObject.GetComponent<DefenseTower>().DisableVisualRange();
        }
    }

    private void SetEnemyCounter()
    {
        enemyCounter.GetComponent<Text>().text = "Killed: " + killedEnemy + "/" + createdEnemy;
    }

    private void CheckIfPathBlocked()
    {
        pathfinder.CalculatePath(new Vector3(wall.transform.position.x, 0, wall.transform.position.z), path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {/*nothing*/}
        else
        {
            RevertPlacedTower(ref lastPlacedTower);
        }
    }

    /* todo: bug if two tower placed in each frame */
    private void RevertPlacedTower(ref GameObject lastPlacedTower)
    {
        SetStatusText("Der Weg darf nicht versperrt werden");
        MakeTowerInnactive(lastPlacedTower);
        objectAttachedOnMouse = lastPlacedTower;
    }

    public void SetStatusText(String text)
    {
        if (ui != null)
        {
            GameObject statusTextObject = new GameObject();
            statusTextObject.AddComponent<Text>();
            statusTextObject.transform.SetParent(ui.transform);
            statusTextObject.transform.SetAsFirstSibling();
            statusTextObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            statusTextObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
            statusTextObject.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);
            statusTextObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, -50);
            statusTextObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 50);
            statusTextObject.GetComponent<Text>().text = text;
            statusTextObject.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            statusTextObject.GetComponent<Text>().color = Color.red;
            statusTextObject.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            statusTextObject.GetComponent<Text>().fontStyle = FontStyle.Bold;
            Destroy(statusTextObject, 3f);
        }
        else
        {
            Debug.LogError("ui is null");
        }
    }

    public void MakeTowerInnactive(GameObject newTower)
    {
        newTower.GetComponent<DefenseTower>().EnableVisualRange();
        newTower.GetComponent<BoxCollider>().enabled = false;
        newTower.GetComponent<DefenseTower>().enabled = false;
        newTower.GetComponent<NavMeshObstacle>().enabled = false;
    }

    public void MakeTowerActive(GameObject newTower)
    {
        newTower.GetComponent<DefenseTower>().DisableVisualRange();
        newTower.GetComponent<BoxCollider>().enabled = true;
        newTower.GetComponent<DefenseTower>().enabled = true;
        newTower.GetComponent<NavMeshObstacle>().enabled = true;
    }

    private void SpawnRandomEnemy()
    {
        SpawnEnemy(new Vector3(45, 2, UnityEngine.Random.Range(-45, 45)));
    }

    private void SpawnEnemy(Vector3 position)
    {
        createdEnemy++;
        //GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.Euler(0, -90, 0));
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.transform.position = position;
        newEnemy.GetComponent<SimpleEnemy>().maxHealth = newEnemy.GetComponent<SimpleEnemy>().InitialHealth * (1 + createdEnemy / 10);
        newEnemy.GetComponent<SimpleEnemy>().dmg = newEnemy.GetComponent<SimpleEnemy>().InitialDmg + createdEnemy;
        newEnemy.GetComponent<SimpleEnemy>().value = (newEnemy.GetComponent<SimpleEnemy>().maxHealth + newEnemy.GetComponent<SimpleEnemy>().dmg) / 2;
    }

    public void EnemyKilled(GameObject enemy)
    {
        foreach (DefenseTower defenseObject in defenseObjects)
        {
            defenseObject.Enemies.Remove(enemy);
        }
        killedEnemy++;
        money += enemy.GetComponent<SimpleEnemy>().value;
    }

    public List<DefenseTower> DefenseObjects
    {
        get
        {
            return defenseObjects;
        }
    }
}
