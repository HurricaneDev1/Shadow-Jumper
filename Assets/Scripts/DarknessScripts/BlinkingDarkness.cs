using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingDarkness : MonoBehaviour
{
    [SerializeField]private bool startInvisible;
    [SerializeField]private float timeBetweenSwaps;

    void Start()
    {
        if(startInvisible){
            swapState();
        }
        InvokeRepeating("swapState", timeBetweenSwaps, timeBetweenSwaps);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void swapState(){
        Collider2D col = GetComponent<CircleCollider2D>();
        SpriteRenderer spriteRen = GetComponent<SpriteRenderer>();
        col.enabled = !col.enabled;
        spriteRen.enabled = !spriteRen.enabled;
    }
}
