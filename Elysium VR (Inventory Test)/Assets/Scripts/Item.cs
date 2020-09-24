using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{

    private bool onTrigger;
    private Camera cam;

    private float mouseZPosition;
    private Vector3 mouseOffSet;
    private float time;
    // всё для визуала селект и деселект методов
    private Renderer selectionRenderer;
    private Material defaultMaterial;
    private Material onTriggerMaterial;
    private Material onSelectedMaterial;

    private Transform configContent;
    private Transform backpack;

    private Rigidbody rg;
    private bool isMouseButtonDown;
    private bool isDrop;
    private bool isOverItem;
    private GameObject inventory;

    // всё для превью и конфига
    private GameObject previewObject;
    private MeshFilter previewMeshFilter;
    private Material previewMaterial;
    private Transform previewTransform;
    private List<Text> configTextList = new List<Text>();

    // необходимые скрипты
    private SendRequestManager sendRequestManager;
    private SetItemImage setItemImage;
    private DefaultData defaultData;
    public ItemData itemData;
    private void Start()
    {
        //кэширование данных
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rg = GetComponent<Rigidbody>();
        rg.mass = itemData.ItemWeight;// задаём массу в соответствии с конфигурацией предмета
        selectionRenderer = GetComponent<Renderer>();
        defaultMaterial = selectionRenderer.material;
        defaultData = GameObject.FindGameObjectWithTag("DefaultData").GetComponent<DefaultData>();
        onTriggerMaterial = defaultData.OnTriggerMaterial;
        onSelectedMaterial = defaultData.OnSelectedMaterial;
        configContent = defaultData.ConfigContent;
        backpack = defaultData.Backpack;
        inventory = defaultData.Inventory;
        setItemImage = defaultData.SetItemImage;
        previewObject = defaultData.PreviewObject;
        sendRequestManager = defaultData.SendRequestManager;
        previewMeshFilter = GetComponent<MeshFilter>();
        previewMaterial = GetComponent<Renderer>().material;
        previewTransform = transform;
        AddTextToList();
        SetItemOnStart();
    }
    void SetItemOnStart() //установка предметов на рюкзак если они в инвентаре
    {
        if (itemData.InInventory)
        {
            rg.isKinematic = true;
            transform.SetParent(backpack);
            transform.localPosition = itemData.PositionOnBackpack;
            transform.localRotation = itemData.RotationOnBackpack;
            setItemImage.SetImageWithInventoryNumber(itemData.IndexOfType, itemData);
        }
    }
    void AddTextToList()// добавление в лист компонентов Text конфигурации предметов на UI панели
    {
        for (int i = 0; i < configContent.childCount; i++)
        {
            configTextList.Add(configContent.GetChild(i).GetComponent<Text>());
        }
    }
    private Vector3 GetMouseWorldPosition()//позиция мыши в мировых координатах
    {
        Vector3 mouseWorldPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        mouseWorldPosition.z = mouseZPosition;
        return cam.ScreenToWorldPoint(mouseWorldPosition);
    }

    private void OnMouseDown()
    {
        isMouseButtonDown = true;// ЛКМ нажата
        mouseZPosition = cam.WorldToScreenPoint(transform.position).z;// назначаем позицию мыши в мировых координатах по оси Z
        mouseOffSet = transform.position - GetMouseWorldPosition();// устанавливаем отступ предмета от позиции мыши
    }
    private void OnMouseDrag()
    {
        if (!itemData.InInventory)//изменение позиции предмета в соответствии с мировыми координатами мыши
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(GetMouseWorldPosition().x + mouseOffSet.x, transform.position.y, GetMouseWorldPosition().z + mouseOffSet.z), 5 * Time.deltaTime);//здесь игнорируется ось Y
        }
    }
    private void OnMouseUp()
    {
        isMouseButtonDown = false;// ЛКМ больше не нажата
        selectionRenderer.material = defaultMaterial; // возращаем родной материал предмету
        if (!itemData.InInventory && onTrigger)// добавляем предмет в инвентарь если он не в инвентаре и находится в триггере рюкзака
        {
            MoveItemFromInventory();
        }
    }
    void MoveItemFromInventory()// метод добавления предмета в инвентарь
    {
        SendRequest("Move To Inventory");// отправляем запрос на сервер с именем события
        rg.isKinematic = true; // делаем предмет неподвижным
        transform.SetParent(backpack);// указываем родителя для предмета
        itemData.InInventory = true;// указываем в конфиге предмета, что предмет в инвентаре
        setItemImage.SetImage(itemData.IndexOfType, itemData.ItemSprite);// устанавливаем иконку на UI панели инвентаря
        isDrop = true;// указываем, что предмет должен двигаться на своё место на рюкзаке
    }
    private void OnMouseOver()
    {
        Select();
    }
    void Select()// селектим предмет
    {
        if (!itemData.InInventory)
        {
            if (onTrigger)
            {
                selectionRenderer.material = onTriggerMaterial; // подсвечиваем предмет красным при наведении мыши, когда он в триггере рюкзака
            }
            else
            {
                selectionRenderer.material = onSelectedMaterial;// подсвечиваем предмет синим при наведении мыши 
            }
        }
        if (itemData.InInventory && inventory.activeSelf == true)
        {
            ShowPreview();
        }
        else
        {
            HidePreview();
        }
    }
    void ShowPreview()// метод включения превью объекта
    {
        selectionRenderer.material = onTriggerMaterial;
        isOverItem = true;
        // показываем превью и задаём ему компоненты выбранного предмета
        previewObject.SetActive(true);// включаем превью
        previewObject.transform.localScale = previewTransform.lossyScale;// устанавливаем размер для превью объекта как у предмета
        previewObject.GetComponent<MeshFilter>().mesh = previewMeshFilter.mesh;// устанавливаем 3D модель как у предмета 
        previewObject.GetComponent<Renderer>().material = previewMaterial;// устанавливаем материал как у предмета
        for (int i = 0; i < itemData.ItemConfig.Length; i++)// выводим конфиг предмета на UI
        {
            configTextList[i].text = itemData.ItemConfig[i];// устанавливаем текст конфигурации на UI панели инвентаря
        }
    }
    void HidePreview()// метод отключения превью объекта
    {
        previewObject.SetActive(false);// отключаем превью предмета, если отключён UI инвентаря
        for (int i = 0; i < itemData.ItemConfig.Length; i++)
        {
            configTextList[i].text = string.Empty;// сбрасываем текст конфигурации на UI панели
        }
    }
    private void OnMouseExit()
    {
        Deselect();
    }
    void Deselect()//деселектим предмет
    {
        if (!isMouseButtonDown)
        {
            selectionRenderer.material = defaultMaterial; // возраващаем родной материал, если не нажата ЛКМ
        }
        previewObject.SetActive(false); // отключаем превью предмета
        isOverItem = false; // указываем, что мышь больше не над предметом
        for (int i = 0; i < itemData.ItemConfig.Length; i++)
        {
            configTextList[i].text = string.Empty;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Backpack"))
        {
            onTrigger = true;
            if(isMouseButtonDown)
            {
                selectionRenderer.material = onTriggerMaterial;// подсвечиваем объект, меняя его материал на красный, если нажата ЛКМ и он находится в триггере
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Backpack"))
        {
            onTrigger = false;
            if (isMouseButtonDown)
            {
                selectionRenderer.material = onSelectedMaterial; // подсвечиваем объект, меняя его материал на синий, если нажата ЛКМ и он находится не в триггере
            }
        }
    }
    void RemoveItemFromInventory()// метод извлечения предмета из инвентаря
    {
        rg.isKinematic = false; // делаем предмет подвижным
        transform.parent = null;// убираем родителя у предмета
        transform.position = backpack.transform.position + (transform.position - backpack.transform.position);// устанавливаем предмету новую позицию в мировых координатах
        itemData.InInventory = false;// устанавливаем в конфиге предмета значение, что он не в инвентаре
        itemData.InventoryNumber = 0;// сбрасываем порядковый номер в инвентаре
        setItemImage.ResetImage(itemData.IndexOfType, itemData);// сбрасываем иконку в UI нвентаре 
        isDrop = false; // указываем, что объект больше не должен перемещаться
        SendRequest("Remove From Inventory");// отправляем POST запрос на сервер с именем события
        isOverItem = false;// указываем, что вызывать метод в Update больше не нужно
    }
    void SendRequest(string eventName)// метод отпрвки запроса
    {
        sendRequestManager.Identificator = itemData.ItemIdentificator;
        sendRequestManager.EventName = eventName;
        sendRequestManager.OnSendRequest.Invoke();
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && isOverItem)// извлекаем предмет, если мышь была над предметом в момент отжатия ЛКМ
        {
            RemoveItemFromInventory();
        }
        if (isDrop)// плавно перемещаем предмет к его месту на рюкзаке
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, itemData.PositionOnBackpack, 5f * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, itemData.RotationOnBackpack, 5f * Time.deltaTime);
        }
    }

}
