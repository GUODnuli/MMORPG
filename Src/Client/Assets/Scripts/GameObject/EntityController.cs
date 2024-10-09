using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using SkillBridge.Message;

public class EntityController : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;
    private AnimatorStateInfo currentBaseState;

    public Entity entity;

    public Vector3 position;
    public Vector3 direction;
    Quaternion rotation;

    public Vector3 lastPosition;
    Quaternion lastRotation;

    public float speed;
    public float animSpeed = 1.5f;
    public float jumpPower = 3.0f;

    public bool isPlayer = false;

    // Use this for initialization
    private void Start()
    {
       if (entity != null)
        {
            this.UpdateTransform();
        }

       if (!this.isPlayer)
        {
            rb.useGravity = false;
        }
    }

    private void UpdateTransform()
    {
        this.position = GameObjectTool.LogicToWorld(entity.position);
        this.direction = GameObjectTool.LogicToWorld(entity.position);

        this.rb.MovePosition(this.position);
        this.transform.forward = this.direction;
        this.lastPosition = this.position;
        this.lastRotation = this.rotation;
    }

    private void OnDestory()
    {
        if (entity != null)
        {
            Debug.LogFormat("{0} OnDestroy: Entity ID: {1}, Entity Pos: {2}, Entity Dir: {3}, Entity Speed: {4}", this.name, entity.entityId, entity.position, entity.direction, entity.speed);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (entity == null)
        {
            return;
        }

        this.entity.OnUpdate(Time.fixedDeltaTime);

        if (!this.isPlayer)
        {
            this.UpdateTransform();
        }
    }

    public void OnEntityEvent(EntityEvent entityEvent)
    {
        switch(entityEvent)
        {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;
        }
    }
}