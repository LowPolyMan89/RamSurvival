using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.Serialization;

public class Multitool : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;

    [SerializeField] private GameObject multitoolBody;
    [SerializeField] private Transform multitoolMountPoint;
    [SerializeField] public GameObject particles;

    private EventManager _eventManager;


    public bool isCollectingActive = false;
    public bool _dooDamageIsRunning = false;
    private Camera _camera;
    public Entity hitEntity;
    public bool isRedy = false;

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(0.2f);
        _camera = Camera.main;
        _eventManager = EventManager.Instance;
        isRedy = true;
    }

    private void Start()
    {
        particles.SetActive(false);
        StartCoroutine(Init());
    }

    private void Update()
    {
        if (!isRedy) return;

        if (hitEntity is Resource)
        {
            if (hitEntity.GetComponent<Resource>().IsCollect)
            {
                hitEntity = null;
            }
        }
        
        if (hitEntity)
        {
            multitoolBody.transform.LookAt(hitEntity.transform);
            
            if (hitEntity is Resource && isCollectingActive)
            {
                particles.SetActive(true);

                if (!_dooDamageIsRunning)
                {
                    StartCoroutine(DooDamage());
                }
            }

        }

        if (hitEntity)
        {
            if (CalculateDistance(hitEntity.transform) > 20)
            {
                hitEntity = null;
                
            }

        }
        else
        {
            isCollectingActive = false;
            particles.SetActive(false);
            multitoolBody.transform.LookAt(shootPoint);
            StopCoroutine(DooDamage());
        }
        
    }

    private float CalculateDistance(Transform target)
    {
        if (!target) return 500f;
        var dist = Vector3.Distance(multitoolBody.transform.position, target.position);
        return dist;
    }


    private IEnumerator DooDamage()
    {
        _dooDamageIsRunning = true;
        yield return new WaitForSeconds(1f);

        if (hitEntity)
        {
            if (hitEntity is Resource)
            {
                hitEntity.GetComponent<Resource>().TakeDamage(15);
            }
        }

        _dooDamageIsRunning = false;
    }
}
