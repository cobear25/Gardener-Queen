using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProjection : MonoBehaviour
{
    public bool hasWorker = false;
    public bool available = false;
    public Building buildingPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddWorker()
    {
        hasWorker = true;
    }

    public void StartBuilding(WorkerType workerType)
    {
        Building building = Instantiate(buildingPrefab);
        building.transform.position = transform.position;
        building.SetType(workerType);
        Destroy(gameObject);
    }
}
