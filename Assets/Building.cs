﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Heart heartPrefab;
    public Worker workerPrefab;
    public Turret turretPrefab;

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
                heart.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                spriteRenderer.color = Color.magenta;
                break;
            case WorkerType.Defense:
                spriteRenderer.color = Color.cyan;
                Turret turret = Instantiate(turretPrefab);
                turret.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                break;
            case WorkerType.Nursery:
                Worker worker = Instantiate(workerPrefab);
                worker.GetComponent<Rigidbody2D>().gravityScale = 0;
                worker.transform.localScale = new Vector3(1, 1, 1);
                worker.building = this;
                worker.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                spriteRenderer.color = new Color(0.88f, 0.53f, 0);
                break;
        }
    }
}
