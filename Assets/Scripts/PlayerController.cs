using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   private Rigidbody playerRb;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    public float jumpForce=10.0f;
    public float gravityModifier;
    public bool isOnGround=true;
    public bool gameOver;
    public bool doubleJumpUsed=false;
    public bool doubleSpeed=false;
    public float doubleJumpForce;
    // Start is called before the first frame update
    void Start()
    {
        playerRb=GetComponent<Rigidbody>();
        playerAnim=GetComponent<Animator>();
        playerAudio=GetComponent<AudioSource>();
        Physics.gravity*=gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver){
            playerRb.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
            isOnGround=false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound,1.0f);
            doubleJumpUsed=false;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && !isOnGround && !doubleJumpUsed){
            doubleJumpUsed=true;
            playerRb.AddForce(Vector3.up*doubleJumpForce,ForceMode.Impulse);
            playerAudio.PlayOneShot(jumpSound,1.0f);
            playerAnim.Play("Running_Jump",3);
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            doubleSpeed=true;
            playerAnim.SetFloat("Speed_Multiplier",2.0f);
        }
        else if(doubleSpeed){
            doubleSpeed=false;
            playerAnim.SetFloat("Speed_Multiplier",1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision){
        
        if(collision.gameObject.CompareTag("Ground")){
            isOnGround=true;
            dirtParticle.Play();
        }
        else if(collision.gameObject.CompareTag("Obstacle")){
            Debug.Log("Game Over!" );
            playerAnim.SetBool("Death_b",true);
            playerAnim.SetInteger("DeathType_int",1);
            gameOver=true;
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound,1.0f);
        }
    }
}
