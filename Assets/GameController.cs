using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public int life = 10;
    public bool isBuilding = false;
    public bool isDeleting = false;

    public Worker workerPrefab;
    public Enemy enemyPrefab;
    //public List<Worker> workers;
    public Text lifeText;
    public Text workerText;
    public GameObject gameOverPanel;
    public GameObject buildingProjectionPrefab;

    private GameObject buildingProjection;

    public bool gameOver = false;

    private float timeSinceEnemyChance = 0.0f;
    public float enemyFrequency = 5.0f;

    void Start()
    {
        //workers = new List<Worker> { };
        gameOverPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            gameOver = true;
            gameOverPanel.SetActive(true);
        }

        if (buildingProjection != null)
        {
            // In build mode
            float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            buildingProjection.transform.position = new Vector2(x, -3.25f);

            if (Input.GetButtonDown("Fire1"))
            {
                // Add building
                GameObject newBuilding = Instantiate(buildingProjectionPrefab);
                newBuilding.transform.position = buildingProjection.transform.position;
                newBuilding.GetComponent<BuildingProjection>().available = true;
                life -= 1;
                // turn build mode off
                EnterBuildMode();
                UpdateUI();
            }
        } else if (isDeleting == true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Worker")
                    {
                        Worker worker = hit.collider.gameObject.GetComponent<Worker>();
                        if (worker.available == true && worker.working == false)
                        {
                            Destroy(worker.gameObject);
                            return;
                        }
                    }
                }
            }
        }
        else {
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Heart")
                    {
                        GameObject heart = hit.collider.gameObject;
                        if (heart.GetComponent<Heart>().available == true)
                        {
                            heart.GetComponent<Heart>().clicked = true;
                            heart.GetComponent<Heart>().available = false;
                            heart.GetComponent<Heart>().building.SetType(WorkerType.Heart);
                            Invoke("UpLife", 0.5f);
                            Destroy(heart, 0.5f);
                        }
                    }
                }
            }
        }
        if (gameOver == false)
        {
            timeSinceEnemyChance += Time.deltaTime;
            if (timeSinceEnemyChance >= enemyFrequency)
            {
                timeSinceEnemyChance = 0;
                int rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    Enemy enemy = Instantiate(enemyPrefab);
                    enemy.transform.position = new Vector2(10, -2.5f);
                    enemy.gameController = this;
                }
                else if (rand == 1)
                {
                    Enemy enemy = Instantiate(enemyPrefab);
                    enemy.transform.position = new Vector2(-10, -2.5f);
                    enemy.gameController = this;
                }
            }
        }
    }

    void UpLife()
    {
        if (life < 10)
        {
            life += 1;
            UpdateUI();
        }
    }

    public void HitByEnemy()
    {
        life -= 1;
        UpdateUI();
    }

    public void UpdateUI()
    {
        lifeText.text = "LIFE: " + life;
        //workerText.text = "WORKERS: " + workers.Count;
    }

    public void AddWorker()
    {
        if (gameOver == false)
        {
            life -= 1;
            Worker worker = Instantiate(workerPrefab);
            worker.gameController = this;
            worker.available = true;
            //workers.Add(worker);
            worker.transform.position = new Vector2(0, 3);
            UpdateUI();
        }
    }

    public void EnterBuildMode()
    {
        if (gameOver == false)
        {
            if (isBuilding)
            {
                buildingProjection = Instantiate(buildingProjectionPrefab);
            } else
            {
                Destroy(buildingProjection);
            }
            isBuilding = !isBuilding;
        }
    }

    public void EnterDeleteMode()
    {
        isDeleting = !isDeleting;
        GameObject[] workers = GameObject.FindGameObjectsWithTag("Worker");
        foreach (GameObject worker in workers)
        {
            if (worker.GetComponent<Worker>() != null)
            {
                worker.GetComponent<Worker>().deleteMode = isDeleting;
            }
        }

    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void GoAgain()
    {
        SceneManager.LoadScene("GameScene");
    }
}
