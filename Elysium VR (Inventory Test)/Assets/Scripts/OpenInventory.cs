using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    [SerializeField] private GameObject InventoryUI = null;
    [SerializeField] private Animation ShowInventoryAnim = null;
    [SerializeField] private GameObject inventory = null;
    private void OnMouseDown()
    {
        InventoryUI.SetActive(true);
        ShowInventoryAnim.Play("Show Inventory");
    }
    private void OnMouseUp()
    {
        inventory.SetActive(false);
    }
}
