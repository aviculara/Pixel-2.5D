using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject grid;

    // Start is called before the first frame update
    void Start()
    {
        grid.SetActive(true);
        canvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
