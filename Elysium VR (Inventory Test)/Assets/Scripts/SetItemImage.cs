using UnityEngine;
using UnityEngine.UI;

public class SetItemImage : MonoBehaviour
{
    private Image image = null;
    public void SetImage(int indexOfType, Sprite itemSprite)//метод назначения иконки
    {
        Transform content = transform.GetChild(indexOfType);// берём объект со слотами по его индексу типа
        for (int i = 0; i < content.childCount; i++)// перебираем слоты
        {
            image = content.GetChild(i).GetChild(0).GetComponent<Image>();// берём у слота иконку
            if (image.sprite == null)// если иконка пустая, то присваиваем переданный в метод спрайт
            {
                image.sprite = itemSprite;
                image.color = new Color(1,1,1,1);// делаем непрозрачность 100%
                break;
            }
        }
    }
    public void ResetImage(int indexOfType, ItemData itemData)//метод сброса иконки
    {
        Transform content = transform.GetChild(indexOfType);// берём объект со слотами по его индексу типа
        image = content.GetChild(itemData.InventoryNumber).GetChild(0).GetComponent<Image>();
        image.sprite = null;
        image.color = new Color(1, 1, 1, 0);// делаем непрозрачность 0%
    }
    public void SetImageWithInventoryNumber(int indexOfType, ItemData itemData)//метод назначения иконки по его порядковому номеру в инвентаре
    {
        Transform content = transform.GetChild(indexOfType);// берём объект со слотами по его индексу типа
        image = content.GetChild(itemData.InventoryNumber).GetChild(0).GetComponent<Image>();// берём у слота в соответствии с его порядковым номером иконку
        image.sprite = itemData.ItemSprite;// назначаем икнку
        image.color = new Color(1, 1, 1, 1);// делаем непрозрачность 100%

    }
}
