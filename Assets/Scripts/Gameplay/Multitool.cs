using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Multitool : MonoBehaviour
{
    [SerializeField] private Transform ShootPoint;
    [SerializeField] private Inventory inventory;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Entity hitEntity;

    [SerializeField] private GameObject multitoolBody;
    [SerializeField] private Transform multitoolMountPoint;
    [SerializeField] private GameObject particles;

    private EventManager eventManager;
    private StarterAssetsInputs _input;

    public bool isCollectingActive = false;
    private bool dooDamageIsRunning = false;

    private void Start()
    {
        eventManager = EventManager.instance;
        _input = GetComponent<StarterAssetsInputs>();
        particles.SetActive(false);
    }

    private void Update()
    {
        if(_input.use)
        {
            if (hitEntity)
            {
                if (hitEntity is Resource)
                {
                    isCollectingActive = true;
                    particles.SetActive(true);

                    if (!dooDamageIsRunning)
                    {
                        StartCoroutine(DooDamage());
                    }
                }
                if (hitEntity is Item && hitEntity.tag != "Resource")
                {
                    _input.UseInput(false);
                    Item _item = hitEntity.GetComponent<Item>();

                    if(_item.ItemType == ItemType.Loot)
                    {
                        inventory.AddItem(_item, _item.Count);
                    }
                    if (_item.ItemType == ItemType.Equip)
                    {
                        inventory.AddEqipItem(_item);
                    }
                }

            }
        }

        if (hitEntity)
        {
            multitoolBody.transform.LookAt(hitEntity.transform);
        }

        else
        {
            isCollectingActive = false;
            particles.SetActive(false);
            _input.UseInput(false);
            StopCoroutine(DooDamage());
        }
    }

    public IEnumerator DooDamage()
    {
        dooDamageIsRunning = true;
        yield return new WaitForSeconds(1f);
        print("Doo Damage " + PlayerStats.instance.PlayerMultitoolData.MiningDPS);

        if(hitEntity)
        {
            if(hitEntity is Resource)
            {
                hitEntity.GetComponent<Resource>().TakeDamage(PlayerStats.instance.PlayerMultitoolData.MiningDPS);
            }
        }
        dooDamageIsRunning = false;
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(Camera.main.transform.position, (ShootPoint.position - Camera.main.transform.position));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, PlayerStats.instance.PlayerMultitoolData.MiningRange, layerMask))
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
        if (!eventManager)
            return;

        eventManager.OnResorceSelect(entity);
        hitEntity = entity;
    }
}
