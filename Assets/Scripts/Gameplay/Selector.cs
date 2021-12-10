using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Selector : MonoBehaviour
{
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

        _uiController = FindObjectOfType<UIController>();
        _uiController.GrabButton.onClick.AddListener(Grab);
        isRedy = true;
    }

    public void Grab()
    {
        if (!hitEntity) return;
        
        switch (hitEntity)
        {
            case Chest _:
                print("Select Chest");
                hitEntity.GetComponent<Chest>().Use(hitEntity.GetComponent<Chest>());
                break;
            case Resource _:
                _multitool.hitEntity = hitEntity;
                _multitool.isCollectingActive = true;
                break;
        }

        if (hitEntity is Item && !hitEntity.CompareTag("Resource"))
        {
            var item = hitEntity.GetComponent<Item>();

            switch (item.ItemType)
            {
                case ItemType.Loot:
                    Player.Instance.PlayerInventory.AddItem(item);
                    break;
                case ItemType.Equip:
                    Player.Instance.PlayerInventory.AddItem(item);
                    break;
                case ItemType.Resource:
                    Player.Instance.PlayerInventory.AddItem(item);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

        if (Physics.Raycast(ray, out var hit, 20, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point);
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
