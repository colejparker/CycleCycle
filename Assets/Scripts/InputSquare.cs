using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSquare : MonoBehaviour
{
    [SerializeField] KeyCode keycode;
    [SerializeField] Image image;
    Color COLOR = new Color(0.71f, 0.71f, 0.71f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(keycode))
        {
            image.color = COLOR;
        } else
        {
            image.color = Color.white;
        }
    }
}
