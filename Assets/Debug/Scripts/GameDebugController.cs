using DemiurgEngine.AI;
using DemiurgEngine.StatSystem;
using InventorySystem;
using UnityEngine;

public class GameDebugController : MonoBehaviour
{
    public string gameObjectName;
    public string command;
    public string arg1;
    public string arg2;
    public string arg3;
    public string arg4;

    public Vector2Int vector2Int;
    public CharacterFacade character;
    public CharacterFacade sender;
    public CharacterFacade targetCharacter;
    [Header("Items")]
    public InventoryController inventory;
    public ItemsCollection itemsCollection;
    public ItemController worldItem;
    public Item item;
    [Header("Stats")]
    public StatsController statsController;

    public bool applyCommand;

    private void Start()
    {
        //create group
        if (GameManager.instance == null)
        {
            return;
        }
        GameManager.instance.group = new();
        int i = 0;
        foreach (var row in GameManager.Characters.Rows)
        {
            GameManager.instance.group.Add(row.character);
            row.numInGroup = i;
            i++;
        }
    }
    private void OnValidate()
    {
        bool commandValid = true;
        int parsedInt = 0;
        Stat stat = null;
        //try
        //{
        if (applyCommand)
        {
            switch (command)
            {
                case "goto":
                    character.GetComponent<Brain>().ApplyCommand(command, vector2Int, sender);
                    break;
                case "create group":
                    GameManager.instance.group = new();
                    int i = 0;
                    foreach (var row in GameManager.Characters.Rows)
                    {
                        GameManager.instance.group.Add(row.character);
                        row.numInGroup = i;
                        i++;
                    }
                    break;
                case "line up":
                    foreach (var row in GameManager.Characters.Rows)
                    {
                        row.character.brain.ApplyCommand(command, vector2Int, arg1);
                    }
                    break;
                case "round dance":
                    foreach (var row in GameManager.Characters.Rows)
                    {
                        row.character.brain.ApplyCommand(command, vector2Int, arg1, arg2);
                    }
                    break;
                case "stop":
                    if (arg1 == "all")
                    {
                        foreach (var row in GameManager.Characters.Rows)
                        {
                            row.character.brain.ApplyCommand(command);
                        }
                    }
                    else
                    {
                        character.brain.ApplyCommand(command);
                    }
                    break;
                case "fight":
                    character.brain.ApplyCommand(command, targetCharacter);
                    break;
                case "attack":
                    character.brain.ApplyCommand(command, targetCharacter);
                    break;
                case "add":
                    int quantity = int.TryParse(arg1, out parsedInt) ? parsedInt : 1;
                    itemsCollection?.Add(item, quantity);
                    break;
                case "remove":
                    itemsCollection?.Remove(item);
                    break;
                case "reduce":
                    itemsCollection?.Reduce(itemsCollection.FindIndex(item), int.Parse(arg1));
                    break;
                case "equip":
                    character.inventory.AddItem(item);
                    character.equip.EquipWeapon(item);
                    break;
                case "throw away":
                    inventory.ThrowItemToWorld(inventory.Collection.FindIndex(item), int.TryParse(arg1, out parsedInt) ? parsedInt : inventory.Collection.Find(item).quantity);
                    break;
                case "pick up":
                    inventory.AddFromWorld(worldItem);
                    break;
                case "add stat":
                    if (int.TryParse(arg2, out parsedInt))
                    {
                        if (arg3 == "global")
                        {
                            stat = GlobalStats.GetGlobalStat(arg1);
                        }
                        else
                        {
                            stat = statsController?.GetStat(arg1);

                        }
                        stat.AddBaseValue(parsedInt, true);
                    }
                    break;
                case "add stat current":
                    if (int.TryParse(arg2, out parsedInt))
                    {
                        if (arg3 == "global")
                        {
                            stat = GlobalStats.GetGlobalStat(arg1);
                        }
                        else
                        {
                            stat = statsController?.GetStat(arg1);

                        }
                        stat.AddCurrentValue(parsedInt, true);
                    }
                    break;

                default:
                    commandValid = false;
                    break;
            }
            if (commandValid)
            {
                Debug.Log("command " + command + "\napplied");
            }
            else
            {
                Debug.LogWarning("command " + command + "\nis not valid!");
            }
        }
        //}
        //catch (Exception)
        //{
        //    //throw new Exception("Ошибка при обработке команды!");
        //    Debug.LogError("Command processing error!");
        //}
    }
}
