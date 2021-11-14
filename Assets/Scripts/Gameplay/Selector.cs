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
    private StarterAssetsInputs _input;
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
        _input = GetComponent<StarterAssetsInputs>();
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
                    _input.UseInput(false);
                }

                if (hitEntity is Item && !hitEntity.CompareTag("Resource"))
                {
                    _input.UseInput(false);
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

        if (_input.use)
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
        var ray = new Ray(position, (shootPoint.position - position));

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
