﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera _MainCamera;
    Vector3 _offset;
    public Transform _defaultParent, _defaultTempCardParent;
    GameObject _tempCardGo;
    void Awake()
    {
        _MainCamera = Camera.allCameras[0];
        _tempCardGo = GameObject.Find("TempCardGo");
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _offset = transform.position - _MainCamera.ScreenToWorldPoint(eventData.position);
        _defaultParent = _defaultTempCardParent = transform.parent;

        _tempCardGo.transform.SetParent(_defaultParent);
        _tempCardGo.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(_defaultParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = _MainCamera.ScreenToWorldPoint(eventData.position);
        newPos.z = 0;
        transform.position = newPos + _offset;
        
        if (_tempCardGo.transform.parent != _defaultTempCardParent)
        {
            _tempCardGo.transform.SetParent(_defaultTempCardParent);
        }

        CheckPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_defaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(_tempCardGo.transform.GetSiblingIndex());
        _tempCardGo.transform.SetParent(GameObject.Find("Canvas").transform);
        _tempCardGo.transform.localPosition = new Vector3(2700, 0);
    }

    void CheckPosition()
    {
        int newIndex = _defaultTempCardParent.childCount;

        for (int i = 0; i < _defaultTempCardParent.childCount; i++)
        {
            if (transform.position.x<_defaultTempCardParent.GetChild(i).position.x)
            {
                newIndex = i;

                if (_tempCardGo.transform.GetSiblingIndex() < newIndex)
                {
                    newIndex--;
                }

                break;
            }
        }
        _tempCardGo.transform.SetSiblingIndex(newIndex);
    }
}