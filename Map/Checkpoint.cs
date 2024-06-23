using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour{

    private GameMaster gm;
    private bool activated = false;
    public Animator animator;
    [SerializeField] AudioSource shineSound;

    private void Start(){
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter2D(Collider2D other){

        //Đặt vị trí checkpoint của trò chơi sau khi người chơi va chạm với checkpoint
        if (other.CompareTag("Player")) {
            if (!activated) {
                shineSound.Play();
                activated = true;
                animator.SetTrigger("activate");
                gm.lastCheckpointPos = transform.position;
            }
        }
    }

    //hủy checkpoint khi va chạm vào checkpoint
    public void destroy() {
        Destroy(gameObject);
    }
}
