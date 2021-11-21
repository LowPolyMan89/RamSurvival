using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class ItemUIElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Item Item;
    public Image Image;
    public Text CountText;
    [SerializeField] private Transform startRoot;

    public void OnEnable()
    {
        StartCoroutine(CustomUpdate());
    }

    private void OnDisable()
    {
        StopCoroutine(CustomUpdate());
    }

    private IEnumerator CustomUpdate()
    {
        yield return new WaitForSeconds(0.1f);
        Image.sprite = Item.Sprite;
        CountText.text = Item.Count.ToString();
        StartCoroutine(CustomUpdate());
    }
    
    
    public void OnDrag(PointerEventData eventData)
    {
#if UNITY_EDITOR
        transform.position = Input.mousePosition;
#endif
 
#if !UNITY_EDITOR
        transform.position = Input.touches[0].position;
#endif
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startRoot = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var uiEventSystem = EventSystem.current;
        
        if (uiEventSystem != null)
        {
            var uiPointerEventData = new PointerEventData(uiEventSystem);
            uiPointerEventData.position = Input.mousePosition;

            List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();

            uiEventSystem.RaycastAll(uiPointerEventData, uiRaycastResultCache);

            if (uiRaycastResultCache.Count > 0)
            {
                foreach (var v in uiRaycastResultCache)
                {
                    if (v.gameObject.CompareTag($"Cell"))
                    {
                        if (v.gameObject.transform.childCount > 0)
                        {
                            ResetPosition();
                            return;
                        }
                        else
                        {
                            transform.SetParent(v.gameObject.transform);
                            transform.localPosition = Vector3.zero;
                        }

                        return;
                    }

                    if (v.gameObject.CompareTag($"ItemSlot"))
                    {
                        if (v.gameObject.transform.childCount > 0)
                        {
                            ResetPosition();
                            return;
                        }
                        else
                        {
                            transform.SetParent(v.gameObject.transform);
                            transform.localPosition = Vector3.zero;
                        }

                        return;
                    }

                    if (v.gameObject.CompareTag($"DropZone"))
                    {

                        transform.SetParent(v.gameObject.transform);
                        transform.localPosition = Vector3.zero;

                        return;
                    }
                    else
                    {
                        ResetPosition();
                    }

                }
            }
            else
            {
                ResetPosition();
            }
        }
    }
    
    private void ResetPosition()
    {
        transform.SetParent(startRoot);
        transform.localPosition = Vector3.zero;
    }
}
