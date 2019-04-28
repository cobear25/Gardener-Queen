using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Heart heartPrefab;
    public Worker workerPrefab;
    public GameObject turretPrefab;

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
                heart.building = this;
                heart.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                spriteRenderer.color = Color.magenta;
                break;
            case WorkerType.Defense:
                spriteRenderer.color = Color.blue;
                break;
            case WorkerType.Nursery:
                Worker worker = Instantiate(workerPrefab);
                worker.GetComponent<Rigidbody2D>().gravityScale = 0;
                worker.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                worker.building = this;
                worker.transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                spriteRenderer.color = Color.green;
                break;
        }
    }
}
