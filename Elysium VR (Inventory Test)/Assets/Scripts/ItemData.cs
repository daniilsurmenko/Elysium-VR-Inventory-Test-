using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New ItemData", menuName = "ItemData", order = 51)]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private float itemWeight = 0;
    [SerializeField]
    private string itemName = null;
    [SerializeField]
    private string itemIdentificator = null;
    [SerializeField]
    private string itemType = null;
    [SerializeField]
    private int inventoryNumber = 0;
    [SerializeField]
    private bool inInventory = false;
    [SerializeField]
    private Sprite itemSpite = null;
    [SerializeField]
    Vector3 positionOnBackpack = new Vector3();
    [SerializeField]
    Quaternion rotationOnBackpack = new Quaternion();
    [SerializeField]
    private int indexOfType = 0;

    public string[] ItemConfig
    {
        get
        {
            string[] itemConfig = {
                "Name: " + itemName,
                "Weight: " + itemWeight.ToString(),
                "Type: " + itemType,
                "Identificator: " + itemIdentificator
            };
            return itemConfig;
        }
    }
    public float ItemWeight
    {
        get
        {
            return itemWeight;
        }
    }
        public string ItemName
    {
        get
        {
            return itemName;
        }
    }
        public string ItemIdentificator
    {
        get
        {
            return itemIdentificator;
        }
    }
        public string ItemType
    {
        get
        {
            return itemType;
        }
    }
    public int InventoryNumber
    {
        get
        {
            return inventoryNumber;
        }
        set
        {
            inventoryNumber = value;
        }
    }
    public bool InInventory
    {
        get
        {
            return inInventory;
        }
        set
        {
            inInventory = value;
        }
    }
    public Sprite ItemSprite
    { 
        get
        {
            return itemSpite;
        }
    }
    public Vector3 PositionOnBackpack
    {
        get
        {
            return positionOnBackpack;
        }
    }
    public Quaternion RotationOnBackpack
    {
        get
        {
            return rotationOnBackpack;
        }
    }
    public int IndexOfType
    {
        get
        {
            return indexOfType;
        }
    }
}
