using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public Vector2 enemyPos = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyPos, 10 * Time.deltaTime);
        if (Mathf.Abs(transform.position.x - enemyPos.x) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
