using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Selector : MonoBehaviour
{
    private Inventory _inventory;
    private Camera _camera;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask layerMask;
    private EventManager _eventManager;
    [SerializeField] private Entity hitEntity;

    [SerializeField] private Multitool _multitool;
    [SerializeField] private UIController _uiController;

    public bool isRedy = false;
    
    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(0.2f);
        // ReSharper disable once Unity.PerformanceCriticalCodeCameraMain
        _camera = Camera.main;
        _eventManager = EventManager.Instance;
        _inventory = PlayerStats.Instance.Inventory;

        _uiController = FindObjectOfType<UIController>();
        _uiController.GrabButton.onClick.AddListener(Grab);
        isRedy = true;
    }

    public void Grab()
    {
        if (hitEntity)
            {
                if (hitEntity is Resource)
                {
                    _multitool.hitEntity = hitEntity;
                    _multitool.isCollectingActive = true;
                }

                if (hitEntity is Inventory)
                {
                    print("Open storage");
                    Inventory storage = hitEntity.GetComponent<Inventory>();
                    PlayerStats.Instance.Inventory.InventoryUI.gameObject.SetActive(true);
                    PlayerStats.Instance.Inventory.InventoryUI.OpenStorageUI(storage);

                }
                
                if (hitEntity is Item && !hitEntity.CompareTag("Resource"))
                {
                    
                    var item = hitEntity.GetComponent<Item>();

                    switch (item.ItemType)
                    {
                        case ItemType.Loot:
                            PlayerStats.Instance.Inventory.AddItem(item, item.Count);
                            break;
                        case ItemType.Equip:
                            PlayerStats.Instance.Inventory.AddEqipItem(item);
                            break;
                        case ItemType.Resource:
                            PlayerStats.Instance.Inventory.AddItem(item, item.Count);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        
    }
    
    private void Update()
    {
        if(!isRedy)
            return;
        _uiController.isGrabObjectFind = hitEntity ? true : false;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Grab();
        }
    }

    private void FixedUpdate()
    {
        if(!isRedy)
            return;
        if (!_camera) return;
        var transform1 = _camera.transform;
        var position = transform1.position;
        var ray = new Ray(position, (shootPoint.position - Camera.main.transform.position));

        if (Physics.Raycast(ray, out var hit, PlayerStats.Instance.PlayerMultitoolData.MiningRange, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point);
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            AddSelectionEntity(hit.collider.GetComponent<Entity>());

        }
        else
        {
            AddSelectionEntity(null);
        }
    }

    private void AddSelectionEntity(Entity entity)
    {
        if (!_eventManager)
            return;

        _eventManager.OnResorceSelect(entity);
        hitEntity = entity;
    }
}
