using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public bool available = false;
    public bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (clicked == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 3), 20 * Time.deltaTime);
        }

        if (transform.localScale.x < 4)
        {
            transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 0.3f;
        } else
        {
            available = true;
        }
    }
}
