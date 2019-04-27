using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerType { Heart = 0, Defense, Nursery };
public class Worker : MonoBehaviour
{
    public GameController gameController;
    public bool working = false;
    public bool available = false;
    private bool dirRight = true;
    public float speed = 2.0f;
    public WorkerType workerType = WorkerType.Heart;
    public Building building;

    private BuildingProjection foundProjection;

    void Start()
    {
        dirRight = Random.Range(0, 2) == 0;
        int randomType = Random.Range(0, 3);
        workerType = (WorkerType)randomType;
        setColor();
        if (available == false)
        {
            Color inactiveColor = GetComponent<SpriteRenderer>().color;
            inactiveColor.a = 0.5f;
            GetComponent<SpriteRenderer>().color = inactiveColor;
        }
    }

    void setColor()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        switch (workerType) {
            case WorkerType.Heart:
                renderer.color = Color.red;
                break;
            case WorkerType.Defense:
                renderer.color = Color.blue;
                break;
            case WorkerType.Nursery:
                renderer.color = Color.green;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (available == true)
        {
            Color activeColor = GetComponent<SpriteRenderer>().color;
            activeColor.a = 1.0f;
            GetComponent<SpriteRenderer>().color = activeColor;
            if (working == false)
            {
                if (dirRight)
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                else
                    transform.Translate(-Vector2.right * speed * Time.deltaTime);

                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2);
                if (hitColliders.Length > 0)
                {
                    int i = 0;
                    while (i < hitColliders.Length)
                    {
                        BuildingProjection projection = hitColliders[i].GetComponent<BuildingProjection>();
                        if (projection != null && projection.available == true && projection.hasWorker == false)
                        {
                            foundProjection = projection;
                            foundProjection.AddWorker();
                            working = true;
                            break;
                        }
                        i++;
                    }
                }
            } else
            {
                Vector2 goToPoint = new Vector2(foundProjection.transform.position.x, foundProjection.transform.position.y + 1);
                transform.position = Vector2.MoveTowards(transform.position, goToPoint, 10 * Time.deltaTime);
                if (Mathf.Abs(transform.position.x - foundProjection.transform.position.x) < 0.1f)
                {
                    foundProjection.StartBuilding(workerType);
                //gameController.workers.Remove(this);
                    //gameController.UpdateUI();
                    Destroy(gameObject);
                }
            }
        } else
        {
            if (transform.localScale.x < 1)
            {
                transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 0.05f;
            }
            else
            {
                Color activeColor = GetComponent<SpriteRenderer>().color;
                activeColor.a = 1.0f;
                GetComponent<SpriteRenderer>().color = activeColor;
                available = true;
                GetComponent<Rigidbody2D>().gravityScale = 2;
                building.SetType(WorkerType.Nursery);
            }
        }
    }

    private void OnBecameInvisible()
    {
        dirRight = !dirRight;
    }
}
