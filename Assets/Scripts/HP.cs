using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        StartCoroutine(ChangeColour());
    }

    IEnumerator ChangeColour()
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(1f, 116f / 255f, 116f / 255f);
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = new Color(1f, 1f, 1f);
    }
}
