using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        _uiController.UseButton.onClick.AddListener(Use);
        _uiController.GrabButton.onClick.AddListener(Grab);
        isRedy = true;
    }

    private void Grab()
    {
        if (_uiController.nearestItems.Count > 0)
        {
            List<Item> toremove = new List<Item>();
            
            foreach (var i in _uiController.nearestItems)
            {
                Item getitem = i;
                Player.Instance.PlayerInventory.AddItem(getitem);
                toremove.Add(i);
            }

            foreach (var i in toremove)
            {
                _uiController.RemoveNearestItem(i);
                Destroy(i, 0.001f);
            }
        }
    }

    public void Use()
    {
        if (!hitEntity) return;
        
        switch (hitEntity)
        {
            case Chest _:
                print("Select Chest");
                hitEntity.GetComponent<Chest>().Use(hitEntity.GetComponent<Chest>());
                break;
            case Crafter _:
                print("Open Crafter");
                Crafter cr = hitEntity.GetComponent<Crafter>();
                UIController.Instance.OpenCraftPanel(cr.Sheme, Player.Instance.PlayerInventory, cr);
                break;
            case Resource _:
                _multitool.hitEntity = hitEntity;
                _multitool.isCollectingActive = true;
                break;
            case BuildingTrigger _:
                BuildingTrigger trigger = hitEntity.GetComponent<BuildingTrigger>();
                UIController.Instance.OpenBuildingsUi(trigger);
                break;
        }

    }
    
    private void Update()
    {
        if(!isRedy)
            return;
        _uiController.isUseObjectFind = hitEntity ? true : false;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Use();
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
            Entity entity = hit.collider.GetComponent<Entity>();
            Item item = entity.gameObject.GetComponent<Item>();

            if (item)
            {
                if (item.ItemType == ItemType.Item || item.ItemType == ItemType.Equip || item.ItemType == ItemType.Loot)
                {
                    AddSelectionEntity(null);
                }
            }
            else
            {
                AddSelectionEntity(entity);
            }
           
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

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();
        if (item)
        {
            _uiController.AddNearestItem(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();
        if (item)
        {
            _uiController.RemoveNearestItem(item);
        }
    }
}
