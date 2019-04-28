using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool shooting = false;
    public int health = 3;

    private float timeSinceShot = 0.0f;
    public float shootFrequency = 2.3f;
    public GameController gameController;

    public EnemyBullet bulletPrefab;

    float destinationX;
    // Start is called before the first frame update
    void Start()
    {
        destinationX = Random.Range(-5.0f, 5.0f); 
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting == false)
        {
            Vector2 goToPoint = new Vector2(destinationX, -1);
            transform.position = Vector2.MoveTowards(transform.position, goToPoint, 3 * Time.deltaTime);
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
        EnemyBullet bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.gameController = gameController;
    }

    public void Hit()
    {
        health -= 1;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
