using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{

    //points for platform to travel to
    public Transform startPoint;
    public Transform endPoint;
    public Transform startPos;
    Vector3 nextPos;

    public float movementSpeed;
    public bool isMoving = true;

    void Start(){
        nextPos = startPos.position; //Di chuyển về phía vị trí ban đầu
    }

    void Update(){

        //Đặt vị trí mục tiêu thích hợp
        if (transform.position == startPoint.position){
            nextPos = endPoint.position;
        }
        else if (transform.position == endPoint.position) {
            nextPos = startPoint.position;
        }

        //Di chuyển nền tảng về phía mục tiêu
        if (isMoving) {
            transform.position = Vector3.MoveTowards(transform.position, nextPos, movementSpeed * Time.deltaTime);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision){
        //Đặt người chơi là một colision để di chuyển chúng cùng với nền tảng
        if (collision.gameObject.CompareTag("Player")) {
            isMoving = true;
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player")){
            collision.collider.transform.SetParent(null);
        }
    }

    //Trực quan đường dẫn nền tảng
    private void OnDrawGizmosSelected(){
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }
}
