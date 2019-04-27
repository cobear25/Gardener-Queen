using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public int life = 10;
    public bool isBuilding = true;

    public Worker workerPrefab;
    //public List<Worker> workers;
    public Text lifeText;
    public Text workerText;
    public GameObject gameOverPanel;
    public GameObject buildingProjectionPrefab;

    private GameObject buildingProjection;

    public bool gameOver = false;
    // Start is called before the first frame update
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
        } else
        {
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
    }

    void UpLife()
    {
        life += 1;
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
