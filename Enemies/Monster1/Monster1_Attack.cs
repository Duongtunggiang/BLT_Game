﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1_Attack : MonoBehaviour{

    public Animator animator;
    public LayerMask players;

    //range of time between attacks
    [SerializeField] float minAttackTimer = 3;
    [SerializeField] float maxAttackTimer = 8;

    //keep track of monster actions
    bool aggro = false;
    bool attacking = false;

    //height and width of rectangular hitbox
    [SerializeField] int attackRangeX = 8;
    [SerializeField] int attackRangeY = 2;

    //offset position of hitbox
    [SerializeField] int attackOffsetX = 4;
    [SerializeField] int attackOffsetY = -2;

    //knockback effect of monster's attack
    [SerializeField] int knockbackX = 6;
    [SerializeField] int knockbackY = 8;
    [SerializeField] float knockbackDuration = 0.8f;

    //time until next attack
    float attackTimer;

    //damage dealt by attack
    public int damage = 30;

    
    void Start(){
        //set an initial timer until next attack
        attackTimer = Random.Range(minAttackTimer, maxAttackTimer + 1);
    }

    void Update(){

        
        if (aggro){ //Khi đến gần quái
            
            //Khi đến gần quái 
            this.GetComponent<Monster_Movement>().stopped = true;
            
            //Tự động tấn công theo thời gian
            if (attackTimer <= 0 && !attacking){
                attackTimer = Random.Range(minAttackTimer, maxAttackTimer + 1);
                attacking = true;
                animator.SetTrigger("attack");
            }
            else{
                attacking = false;
                attackTimer -= Time.deltaTime;
            }

            //Đẩy người chơi ra sau
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
                if (GameObject.Find("Character").GetComponent<PlayerInput>().transform.position.x > transform.position.x ){
                    this.GetComponent<Monster_Movement>().direction = 1;
                }
                else{
                    this.GetComponent<Monster_Movement>().direction = -1;
                }
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //Tự động tấn công khi chạm vào người chơi
        if (other.CompareTag("Player")){
            aggro = true;
            attackTimer = Random.Range(minAttackTimer, maxAttackTimer + 1);
        }

    }

    private void OnTriggerExit2D(Collider2D other){
        
        //reset trạng thái tấn công khi người chơi ra khỏi quái
        if (other.CompareTag("Player")){
            aggro = false;
            attackTimer = Random.Range(minAttackTimer, maxAttackTimer + 1);

            //Trả về trạng thái quái di chuyển ngẫu nhiên
            if (Random.Range(-1, 1) < 0){
                this.GetComponent<Monster_Movement>().direction = -1;
            }
            else{
                this.GetComponent<Monster_Movement>().direction = 1;
            }
        }
    }

    //Tạo một box tìm kiếm người chơi để gây sát thương và gõ
    void hitbox(){

        //Tạo box người chơi
        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(transform.position + new Vector3(attackOffsetX * this.GetComponent<Monster_Movement>().direction, attackOffsetY, 0), new Vector2(attackRangeX, attackRangeY), 0f, players);

        //Áp dụng đòn đánh và hiệu ứng đòn đánh vào người chơi
        foreach (Collider2D player in hitPlayers){
            Debug.Log("Monster_Movement hit" + player.name);

            //apply X knockback: Người chơi bị giật lùi về phía sau khi bị tấn công
            player.GetComponent<PlayerInput>().modX = knockbackX * this.GetComponent<Monster_Movement>().direction;
            player.GetComponent<PlayerInput>().modTimer = knockbackDuration;

            player.GetComponent<PlayerHealth>().takeDamage(damage);

            //apply Y knockback: Người chơi bị nhảy lên khi bị tấn công
            player.attachedRigidbody.velocity = new Vector2(player.attachedRigidbody.velocity.x, knockbackY);
        }

    }

    //Trực quan hóa Hitbox
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(attackOffsetX * this.GetComponent<Monster_Movement>().direction, attackOffsetY, 0), new Vector3(attackRangeX, attackRangeY, 1));
    }

}
