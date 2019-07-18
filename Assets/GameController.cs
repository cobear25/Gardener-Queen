using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public int life = 15;
    public bool isBuilding = false;
    public bool isDeleting = false;

    public Worker workerPrefab;
    public Enemy enemyPrefab;
    //public List<Worker> workers;
    public Text lifeText;
    public Text workerText;
    public Text gameoverScoreText;
    public GameObject gameOverPanel;
    public GameObject buildingProjectionPrefab;
    public Button buildButton;
    public Button consumeButton;
    public SpriteRenderer queen;
    public AudioClip barfSound;
    public AudioClip gruntSound;

    public bool onUI = false;

    private GameObject buildingProjection;

    public bool gameOver = false;

    private float timeSinceEnemyChance = 0.0f;
    public float enemyFrequency = 5.0f;

    int score = 0;

    void Start()
    {
        //workers = new List<Worker> { };
        gameOverPanel.gameObject.SetActive(false);
    }

    int enemyChance = 4;

    // Update is called once per frame
    void Update()
    { 
        if (gameOver == true)
        {
            return;
        }
        if (life <= 0)
        {
            gameoverScoreText.text = "SCORE: " + score;
            gameOver = true;
            gameOverPanel.SetActive(true);
        }

        if (buildingProjection != null)
        {
            // In build mode
            float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            buildingProjection.transform.position = new Vector3(x, -2.95f, 0f);

            //buildButton.GetComponent<Image>().color = Color.Lerp(Color.red, Color.red, Mathf.PingPong(Time.time, 1));
            if (Input.GetButtonDown("Fire1") && onUI == false)
            {
                // Add building
                GameObject newBuilding = Instantiate(buildingProjectionPrefab);
                newBuilding.transform.position = buildingProjection.transform.position;
                newBuilding.GetComponent<BuildingProjection>().available = true;
                newBuilding.GetComponent<BuildingProjection>().gameController = this;
                //life -= 1;
                //queen.color = Color.red;
                //Invoke("ColorBack", 0.2f);
                // turn build mode off
                EnterBuildMode();
                UpdateUI();
            }
        } else if (isDeleting == true)
        {
            if (Input.GetButtonDown("Fire1") && onUI == false)
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
                    } else
                    {
                        EnterDeleteMode();
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
                int rand = Random.Range(0, enemyChance);
                if (rand == 0)
                {
                    Enemy enemy = Instantiate(enemyPrefab);
                    enemy.transform.position = new Vector3(10, -2.5f, -2f);
                    enemy.gameController = this;
                    if (enemyFrequency > 0.5f)
                    {
                        enemyFrequency -= 0.1f;
                    } else
                    {
                        enemyChance = 2;
                    }
                }
                else if (rand == 1)
                {
                    Enemy enemy = Instantiate(enemyPrefab);
                    enemy.transform.position = new Vector3(-10, -2.5f, -2f);
                    enemy.gameController = this;
                    if (enemyFrequency > 0.5f)
                    {
                        enemyFrequency -= 0.1f;
                    }
                    else
                    {
                        enemyChance = 2;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            AddWorker();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            EnterBuildMode();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            EnterDeleteMode();
        }
    }

    void UpLife()
    {
        if (life < 15)
        {
            life += 1;
            UpdateUI();
        }
    }

    public void HitByEnemy()
    {
        life -= 1;
        GetComponent<AudioSource>().PlayOneShot(gruntSound);
        queen.color = Color.red;
        Invoke("ColorBack", 0.2f);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (gameOver == true)
        {
            return;
        }
        lifeText.text = "LIFE: " + life;
        workerText.text = "SCORE: " + score;
        //workerText.text = "WORKERS: " + workers.Count;
    }

    public void AddWorker()
    {
        if (gameOver == false)
        {
            GetComponent<AudioSource>().PlayOneShot(barfSound);
            life -= 1;
            queen.color = Color.red;
            Invoke("ColorBack", 0.2f);
            Worker worker = Instantiate(workerPrefab);
            worker.gameController = this;
            worker.available = true;
            //workers.Add(worker);
            worker.transform.position = new Vector3(-0.11f, 1.94f, -1f);
            UpdateUI();
            if (isDeleting == true)
            {
                // Exit delete mode
                EnterDeleteMode();
            }
            if (isBuilding == true)
            {
                // Exit build mode
                EnterBuildMode();
            }
        }
    }

    void ColorBack()
    {
        queen.color = Color.white;
    }

    public void EnterBuildMode()
    {
        if (gameOver == false)
        {
            isBuilding = !isBuilding;
            if (isBuilding)
            {
                buildingProjection = Instantiate(buildingProjectionPrefab);
                buildButton.GetComponentInChildren<Text>().color = Color.black;
            } else
            {
                buildButton.GetComponentInChildren<Text>().color = Color.white;
                Destroy(buildingProjection);
            }
            if (isDeleting == true)
            {
                // Exit delete mode
                EnterDeleteMode();
            }
        }
    }

    public void EnterDeleteMode()
    {
        if (gameOver == false)
        {
            isDeleting = !isDeleting;
            GameObject[] workers = GameObject.FindGameObjectsWithTag("Worker");
            foreach (GameObject worker in workers)
            {
                if (worker.GetComponent<Worker>() != null && worker.GetComponent<Worker>().available == true)
                {
                    worker.GetComponent<Worker>().deleteMode = isDeleting;
                }
            }

            if (isDeleting == true)
            {
                consumeButton.GetComponentInChildren<Text>().color = Color.black;
            }
            else
            {
                consumeButton.GetComponentInChildren<Text>().color = Color.white;
            }
            if (isBuilding == true)
            {
                // Exit build mode
                EnterBuildMode();
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

    public void OnUI()
    {
        onUI = true;
    }

    public void OffUI() 
    {
        onUI = false;
    }

    public void EnemyDestroyed()
    {
        score += 1;
        UpdateUI();
    }
    public void BuildingBuilt()
    {
        score += 1;
        UpdateUI();
    }

}
