using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private float timeSinceShot = 0.0f;
    float shootFrequency = 1.5f;
    public TurretBullet bulletPrefab;
    public AudioClip shootSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 3);
        if (hitColliders.Length > 0)
        {
            int i = 0;
            while (i < hitColliders.Length)
            {
                Enemy enemy = hitColliders[i].GetComponent<Enemy>();
                if (enemy != null)
                {
                    ShootEnemy(enemy);
                    break;
                }
                i++;
            }
        }
    }

    void ShootEnemy(Enemy enemy)
    {
        timeSinceShot += Time.deltaTime;
        if (timeSinceShot >= shootFrequency)
        {
            GetComponent<AudioSource>().PlayOneShot(shootSound);
            timeSinceShot = 0;
            TurretBullet bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.enemyPos = enemy.transform.position;
            enemy.Hit();
        }
    }
}
