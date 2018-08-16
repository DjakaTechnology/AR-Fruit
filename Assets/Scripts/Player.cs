using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class Player : MonoBehaviour {
    public LeftJoystick leftJoystick; // the game object containing the LeftJoystick script
    public RightJoystick rightJoystick; // the game object containing the RightJoystick script

    public EZObjectPool bullets;
    public Transform outPosition;

    public float gravity = 20f;
    public float jumpForce = 8f;
    public float speed = 6f;
    public float turnSpeed = 5f;
    public float invicibleTime = 3;
    public float shootFreq = 1;

    public GameObject mainCamera;
    public MiniGame miniGameManager;

    private GameObject obj;

    private float invicibleCooldown = 0;
    private float shootCooldown = 0;
    private float v, h, rV, rH;

    private bool canShoot = false;
    private bool isInvicible = false;
    private CharacterController chara;
    private Vector3 moveDirection = Vector3.zero, cameraForward, cameraRight, lookDirection = Vector3.zero;

    void Start() {
        chara = GetComponent<CharacterController>();
    }

    void Update() {
        h = leftJoystick.GetInputDirection().x;
        v = leftJoystick.GetInputDirection().y;
        rH = rightJoystick.GetInputDirection().x;
        rV = rightJoystick.GetInputDirection().y;

        cameraForward = mainCamera.transform.forward;
        cameraRight = mainCamera.transform.right;

        cameraForward.Normalize();

        if (chara.isGrounded) {
            moveDirection = (h * cameraRight + v * cameraForward) * speed;
        }

        lookDirection = (rH * cameraRight + rV * cameraForward) * speed;

        if (rH != 0 || rV != 0) {
            //transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
            Quaternion rotate = Quaternion.LookRotation(lookDirection);
            rotate.x = 0;
            rotate.z = 0;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * turnSpeed);
        }

        moveDirection.x *= 2;
        moveDirection.y = 0;
        moveDirection.y -= gravity * Time.deltaTime;
        chara.Move(moveDirection * Time.deltaTime);

        if (isInvicible) {
            invicibleCooldown += Time.deltaTime;
            if (invicibleCooldown >= invicibleTime) {
                isInvicible = false;
            }
        }

        if (canShoot) {
            Shoot();
        } else {
            shootCooldown += Time.deltaTime;
            if (shootCooldown >= shootFreq)
                canShoot = true;
        }
    }

    void Shoot() {
        if(bullets.TryGetNextObject(outPosition.position, transform.rotation, out obj)) {
            obj.GetComponent<Rigidbody>().AddForce((obj.transform.right * -1) * 1000f);
        }
        canShoot = false;
        shootCooldown = 0;
    }

    void RotatePlayer(Vector3 target) {
        Vector3 relativePos = mainCamera.transform.TransformDirection(target);
        relativePos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(target);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
    }

    private void OnCollisionEnter(Collision collision) {
        if (isInvicible)
            return;
        if (collision.gameObject.CompareTag("Enemy")){
            isInvicible = true;
            miniGameManager.LoseHealth();
            invicibleCooldown = 0;
        }
    }
}
