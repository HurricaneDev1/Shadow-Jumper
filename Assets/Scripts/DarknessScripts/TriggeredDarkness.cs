using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredDarkness : MonoBehaviour
{
    private Vector3 spawnPosition;
    [SerializeField]private GameObject triggerDarkness;
    public GameObject waypoint;
    private float movingSpeed = 0;
    [SerializeField] private float speed = 2f;
    void Start(){
        spawnPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoint.transform.position, Time.deltaTime * movingSpeed);
        if(Vector2.Distance(transform.position , waypoint.transform.position) < .1f){
            MakeNewDarkness();
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Player"){
            movingSpeed = speed;
        }
    }

    public void MakeNewDarkness(){
        GameObject newDarkness = Instantiate(triggerDarkness, spawnPosition,Quaternion.identity);
        newDarkness.GetComponent<TriggeredDarkness>().waypoint = waypoint;
        Destroy(gameObject);
    }
}
