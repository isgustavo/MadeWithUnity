using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class InventoryItem
{
    public int itemID;
    public int count;
    public GameObject ItemPrefab;
}

public interface IPlayerInventory
{
    event Action<InventoryItem> OnCurrentItemChanged;
}

public class PlayerInventory : MonoBehaviour, IPlayerInventory
{
    static readonly int EQUIP_PARAMETER_HASH = Animator.StringToHash("Equip");
    static readonly string SLOT_PREFIX_NAME = "SlotItem";
    static readonly string HAND_SLOT_PREFIX_NAME = "HandSlotItem";

    [SerializeField]
    List<InventoryItem> weaponInventory = new List<InventoryItem>();

    Dictionary<int, InventoryItem> inventoryItens = new Dictionary<int, InventoryItem>();
    Dictionary<int, GameObject> visualItem = new Dictionary<int, GameObject>();

    InventoryItem previousItem;
    InventoryItem currentItem;

    SlotRefs slotsRefs;
    Animator animator;

    bool isSwitchBetweenItens;
    int nextItemID;
    public event Action<InventoryItem> OnCurrentItemChanged;

    void Start() 
    {
        TryGetComponents();
        InitValues();    
    }

    void TryGetComponents()
    {
        animator  = GetComponent<Animator>();
        if(animator == null)
            Debug.LogError("Player inventory controller animator is missing");

        slotsRefs = GetComponentInChildren<SlotRefs>();
        if(slotsRefs == null)
            Debug.LogError("Player inventory controller slotsRefs is missing");
    }
    void  InitValues()
    {
        foreach (InventoryItem inventoryItem in weaponInventory)
        {
            inventoryItens.Add(inventoryItem.itemID, inventoryItem);
            if(inventoryItem.ItemPrefab != null)
                CreateVisual(inventoryItem);
        }

        inventoryItens.TryGetValue(1, out previousItem);
    }

    void CreateVisual(InventoryItem item)
    {
        GameObject newWeapon = Instantiate(item.ItemPrefab, Vector3.one * 2000, Quaternion.identity);//?.GetComponent<WeaponItem>();
        //Debug.Log($"new weapon{newWeapon != null}");
        if(newWeapon != null)
        {
            TrySetParent(item.itemID, newWeapon, SLOT_PREFIX_NAME);
            visualItem.Add(item.itemID, newWeapon);
        }   
    }

    public void OnDPad(InputValue value)
    {
        if(value.Get<Vector2>() == Vector2.right)
            TryEquipItem(1);
        else if(value.Get<Vector2>() == Vector2.left)
            TryEquipItem(2);
    }

    public void OnLeftTrigger(InputValue value)
    {
        if(value.isPressed && currentItem == null)
        {
            TryEquipItem(previousItem.itemID); 
        }
            
    }

    void TryEquipItem(int itemID)
    {
        if(currentItem == null)
        {
            animator.SetInteger(EQUIP_PARAMETER_HASH, itemID); 
        } else if(currentItem.itemID != itemID)
        {
            isSwitchBetweenItens = true;
            nextItemID = itemID;
            animator.SetInteger(EQUIP_PARAMETER_HASH, 0);  
        } else 
        {
            animator.SetInteger(EQUIP_PARAMETER_HASH, 0);
        }
    }

    public void OnEquipInventoryAnimationEvent(int itemID)
    {
        EquipItem(itemID);
    }

    public void OnUnEquipInventoryAnimationEvent(int itemID)
    {
        UnEquipItem(itemID);
        if(isSwitchBetweenItens)
        {
            animator.SetInteger(EQUIP_PARAMETER_HASH, nextItemID);
            isSwitchBetweenItens = false;
        }
    }

    void EquipItem(int itemID)
    {        
        if(inventoryItens.ContainsKey(itemID))
            currentItem = inventoryItens[itemID];
        else 
            Debug.LogError($"Player Inventory Controller Equip missing item {itemID}");    

        if(visualItem.ContainsKey(itemID))
            TrySetParent(itemID, visualItem[itemID], HAND_SLOT_PREFIX_NAME);

        OnCurrentItemChanged?.Invoke(currentItem);    
    }

    void UnEquipItem(int itemID)
    {
        currentItem = null;
        if(inventoryItens.ContainsKey(itemID))
            previousItem = inventoryItens[itemID];
        else 
            Debug.LogError($"Player Inventory Controller unEquip missing item {itemID}");   

        if(visualItem.ContainsKey(itemID))
            TrySetParent(itemID, visualItem[itemID], SLOT_PREFIX_NAME);

        OnCurrentItemChanged?.Invoke(null);    
    }

    void TrySetParent(int itemID, GameObject item, string slotPrefix)
    {
        Transform slot = slotsRefs.TryGetParentWithName($"{slotPrefix}{itemID}");
        if(slot != null)
        {
            item.transform.SetParent(slot);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        } else 
        {
            Debug.LogError($"Player Inventory Controller missing slot with name {slotPrefix}{itemID}");    
        }
    }
}
