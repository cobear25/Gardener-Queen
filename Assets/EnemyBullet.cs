using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 goToPoint = new Vector2(transform.position.x, 3);
        transform.position = Vector2.MoveTowards(transform.position, goToPoint, 10 * Time.deltaTime);
        if (transform.position.y >= 2.99f)
        {
            gameController.HitByEnemy();
            Destroy(gameObject);
        }
    }
}
