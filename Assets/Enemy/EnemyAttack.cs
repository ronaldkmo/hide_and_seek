using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;

    [SerializeField] Transform playerTransform;
    Transform enemyTransform;
    [SerializeField] float maxDistanceToTarget = 6f;

    [SerializeField] float moveSpeed = 1f;
    
    [SerializeField] float delayTimer = 2f;
    
    float tick;
    void Start() {
        tick = delayTimer;
        enemyTransform = gameObject.transform.Find("EnemyModel");
    }
    void Update() {
        Attack();
    }
    
    bool IsReadyToAttack() {
        if (tick < delayTimer) {
            tick += Time.deltaTime;
            return false;
        }
        return true;
    }
 
    void Attack() {
        float distanceToTarget = Vector3.Distance(playerTransform.position, enemyTransform.position);
        
        bool attackReady = IsReadyToAttack();
        if (distanceToTarget <= maxDistanceToTarget)  {

            // Chase the player :)
            Vector3 lookVector = playerTransform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookVector);
            rotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.01f);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.name.Equals("Ground")) {
            if (collision.gameObject.name.Equals("PlayerModel")){
                #if UNITY_STANDALONE
                    Application.Quit();
                #endif
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}