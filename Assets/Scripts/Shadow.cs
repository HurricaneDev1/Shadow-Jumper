using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shadow : MonoBehaviour
{
    [SerializeField]private Vector3 darknessDirection;
    [SerializeField]private LayerMask darkness;
    [SerializeField]private float moveSpeed;
    [SerializeField]private float inShadowDrag;
    [SerializeField]private float dashSpeed;
    [SerializeField]private Vector3 lastRespawn;
    private Rigidbody2D rb;
    [SerializeField]private Animator leftSquare;
    [SerializeField]private Animator rightSquare;
    [SerializeField]private ParticleSystem deathParticles;
    [SerializeField]private CinemachineVirtualCamera cinCamera; 
    [SerializeField]private int deathCounter;
    private Player player;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Physics2D.Raycast(transform.position + darknessDirection, Vector3.forward, 1, darkness)){
            rb.gravityScale = 0;
            rb.drag = inShadowDrag;
            Move();
        }
    }

    void Move(){
        // if(Input.GetKey(KeyCode.W)){
        //     rb.AddForce(Vector2.up * moveSpeed);
        // }else if(Input.GetKey(KeyCode.S)){
        //     rb.AddForce(Vector2.down * moveSpeed);
        // }
        //Allows the player to move up and down in shadows
        if(Input.GetAxisRaw("Vertical") < 0){
            rb.AddForce(Vector2.up * -1 * moveSpeed);
        }else if(Input.GetAxisRaw("Vertical") > 0){
            rb.AddForce(Vector2.up * 1 * moveSpeed);
        }
    }

    void OnDrawGizmos(){
        Gizmos.DrawRay(transform.position + darknessDirection, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Light"){
            StartCoroutine(Death());
        }else if(col.tag == "Respawn"){
            lastRespawn = col.transform.position;
        }
    }

    IEnumerator Death(){
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        //Before Death
        foreach(SpriteRenderer sprite in spriteRenderers){
            sprite.enabled = false;
        }
        player.dead = true;
        rb.velocity = new Vector2(0,0);
        deathParticles.Play();
        //Death transition
        leftSquare.SetTrigger("GoIn");
        rightSquare.SetTrigger("GoIn");
        yield return new WaitForSeconds(0.4f);
        //Death Transition
        rightSquare.ResetTrigger("GoIn");
        leftSquare.ResetTrigger("GoIn");
        foreach(SpriteRenderer sprite in spriteRenderers){
            sprite.enabled = true;
        }
        transform.position = lastRespawn;    
        //Makes the camera go to respawn immediately
        cinCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0;
        yield return new WaitForSeconds(0.05f);
        cinCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 1;
        //Stops the player if they have velocity
        rb.velocity = new Vector2(0,0);
        TriggeredDarkness[] triggereds = FindObjectsOfType<TriggeredDarkness>();
        foreach(TriggeredDarkness trigger in triggereds){
            trigger.MakeNewDarkness();
        }
        yield return new WaitForSeconds(0.5f);
        deathCounter ++;
        player.dead = false;
    }
}
