using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class EnemyAttacv2 : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;

    [SerializeField] Transform playerTransform;
    Transform enemyTransform;
    [SerializeField] float moveSpeed = 1f;
    
    [SerializeField] float delayTimer = 2f;
    [SerializeField] private float fovDist = 3f;
    [SerializeField] private float fovAngle = 75; 
    
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

        // the vector that specifies the direction of the CCTV and the joker
        Vector3 lookVector = playerTransform.position - transform.position;
        //float dotValue = Vector3.Dot(lookVector, transform.forward);
        
        float normDotValue = Vector3.Dot(lookVector.normalized, transform.forward.normalized);
        float angle = Mathf.Rad2Deg * Mathf.Acos(normDotValue);

        if (lookVector.magnitude <= fovDist && lookVector.magnitude > 0 && angle <= fovAngle/2) {
            // Chase the player :)
            // can also use transform.LookAt(playerTransform);
            lookVector.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookVector);
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

    private void OnDrawGizmos()
    {        
        Handles.color = new Color(1, 1, 0, 0.3f);
        Vector3 currentPos = transform.position;
        currentPos.y += 1.5f;
        Handles.DrawSolidArc(currentPos, transform.up, transform.forward, fovAngle/2, fovDist);
        Handles.DrawSolidArc(currentPos, -transform.up, transform.forward, fovAngle/2, fovDist);

    }
}