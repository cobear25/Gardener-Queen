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
    public bool deleteMode = false;

    private BuildingProjection foundProjection;

    void Start()
    {
        dirRight = Random.Range(0, 2) == 0;
        int randomType = Random.Range(0, 3);
        workerType = (WorkerType)randomType;
        SetColor();
        if (available == false)
        {
            Color inactiveColor = GetComponent<SpriteRenderer>().color;
            inactiveColor.a = 0.5f;
            GetComponent<SpriteRenderer>().color = inactiveColor;
        }
    }

    public void SetColor()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        switch (workerType) {
            case WorkerType.Heart:
                renderer.color = Color.magenta;
                break;
            case WorkerType.Defense:
                renderer.color = Color.cyan;
                break;
            case WorkerType.Nursery:
                renderer.color = new Color(0.88f, 0.53f, 0);
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
                if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) < 2)
                {
                    if (dirRight)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                        GetComponent<Rigidbody2D>().AddForce(Vector3.right * 20);
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                        GetComponent<Rigidbody2D>().AddForce(Vector3.right * -20);
                    }
                }
                // check if a building projection is nearby
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
                // Hop to projected building, start building, and destroy self
                Vector2 goToPoint = new Vector2(foundProjection.transform.position.x, foundProjection.transform.position.y + 1);
                transform.position = Vector2.MoveTowards(transform.position, goToPoint, 10 * Time.deltaTime);
                if (Mathf.Abs(transform.position.x - foundProjection.transform.position.x) < 0.1f)
                {
                    foundProjection.StartBuilding(workerType);
                    Destroy(gameObject);
                }
            }
        } else
        {
            if (transform.localScale.x < 2.5f)
            {
                transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 0.1f;
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
        if (deleteMode)
        {
            GetComponent<SpriteRenderer>().color = Color.Lerp(Worker.ColorForWorkerType(workerType), Color.red, Mathf.PingPong(Time.time, 1));
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Worker.ColorForWorkerType(workerType);
        }
    }

    private void OnBecameInvisible()
    {
        dirRight = !dirRight;
    }

    public static Color ColorForWorkerType(WorkerType workerType)
    {
        switch (workerType)
        {
            case WorkerType.Heart:
                return Color.magenta;
            case WorkerType.Defense:
                return Color.cyan;
            case WorkerType.Nursery:
                return new Color(0.88f, 0.53f, 0);
            default:
                return Color.white;
        }
    }
}
