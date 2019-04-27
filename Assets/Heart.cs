using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public bool available = false;
    public bool clicked = false;
    public Building building;
    // Start is called before the first frame update
    void Start()
    {
        Color inactiveColor = Color.white;
        inactiveColor.a = 0.5f;
        GetComponent<SpriteRenderer>().color = inactiveColor;
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
            Color activeColor = Color.white;
            activeColor.a = 1.0f;
            GetComponent<SpriteRenderer>().color = activeColor;
            available = true;
        }
    }
}
