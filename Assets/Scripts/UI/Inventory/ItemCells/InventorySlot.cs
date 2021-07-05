﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected Image m_image;

    public Item_SO Item { get; set; }

    public void SetItem(Item_SO item)
    {
        Item = item;
        m_image.sprite = item.sprite;
        OnItemSetted();
    }

    public void ClearSlot()
    {
        Item = null;
        m_image.sprite = null;
        OnItemDeleted();
    }

    public abstract void OnItemSetted();
    public abstract void OnItemDeleted();
    public abstract Action<Vector2, InventorySlot> GetActionToInvokeOnClick();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Item == null) { return; }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GetActionToInvokeOnClick().Invoke(eventData.position, this);
        }
    }
}