using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool shooting = false;
    public int health = 3;

    private float timeSinceShot = 0.0f;
    float shootFrequency = 4.3f;
    public GameController gameController;

    public EnemyBullet bulletPrefab;
    public AudioClip shootSound;

    float destinationX;
    // Start is called before the first frame update
    void Start()
    {
        destinationX = Random.Range(-8.0f, 8.0f); 
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting == false)
        {
            // Go to a random spot on the screen
            Vector3 goToPoint = new Vector3(destinationX, -1, -2);
            transform.position = Vector3.MoveTowards(transform.position, goToPoint, 3 * Time.deltaTime);
            if (Mathf.Abs(transform.position.x - destinationX) <= 0.2)
            {
                shooting = true;
                Invoke("Shoot", 0.2f);
            }
        } else
        {
            timeSinceShot += Time.deltaTime;
            if (timeSinceShot >= shootFrequency)
            {
                timeSinceShot = 0;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (gameController.gameOver == true) 
        {
            return;
        }
        GetComponent<AudioSource>().PlayOneShot(shootSound);
        EnemyBullet bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.gameController = gameController;
    }

    public void Hit()
    {
        Invoke("RealHit", 0.2f);
    }
    void RealHit()
    {
        health -= 1;
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ColorBack", 0.2f);
        if (health <= 0)
        {
            gameController.EnemyDestroyed();
            Destroy(gameObject);
        }
    }

    void ColorBack()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
