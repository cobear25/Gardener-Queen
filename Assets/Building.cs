using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Heart heartPrefab;

    public WorkerType workerType = WorkerType.Heart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetType(WorkerType workerType)
    {
        this.workerType = workerType;
        // Set sprite
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (workerType)
        {
            case WorkerType.Heart:
                Heart heart = Instantiate(heartPrefab);
                heart.transform.position = transform.position;
                spriteRenderer.color = Color.red;
                break;
            case WorkerType.Defense:
                spriteRenderer.color = Color.blue;
                break;
            case WorkerType.Nursery:
                spriteRenderer.color = Color.green;
                break;
        }
    }
}
