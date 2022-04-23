using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum EnemyState
{
    walk, attack, stagger, dead, idle
}

public class Enemy : Character , IMovable
{
    protected EnemyState currentState;//Enemy state machine
    protected Animation weapon;//will get the animations to the weapon attatched to this character
    protected float chaseRange;//this is the radius where the enemy will begin to chase the player
    protected float attackRange;//this is the radius where the enemy will attack the player
    protected float backOffRange;//this is the radius where the enemy will back off and make room
    protected Vector2 homePosition;//this will store the spawn point of the enemy
    protected GameObject[] players;//this will be an array that will store all the players gameobjects
    protected GameObject target;//this will be what is targed by the enemy
    protected float rotationRadius = 2f;
    protected float posX, posY, angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        this.dexterity = .08f;//sets the movespeed of the enemy
        players = GameObject.FindGameObjectsWithTag("Player");//loads the array with all players in the scene
        currentState = EnemyState.walk;//initialize enemy state with walk
        chaseRange = 8f;//sets the chase radius of enemy
        attackRange = 2.3f;//sets the radius in which the enemy will attack
        backOffRange = 2f;//sets the radius where enemy will back off
        homePosition = this.transform.position;
        target = players[0];
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        FindClosestPlayer();
    }
 
    private void FindClosestPlayer()//This needs to figure which player is closest
    {        
        for(int i = 0; i < players.Length ; i++)
        {
            if (Vector2.Distance(transform.position, target.transform.position) > Vector2.Distance(transform.position, players[i].transform.position))
            {
                target = players[i];
            }
            
        }        
    }
   
    public void Move()
    {
        Vector2 temp;
        Vector2 castV3;
       
        if (currentState != EnemyState.dead && Vector2.Distance(transform.position, target.transform.position) <= chaseRange
             && Vector2.Distance(transform.position, target.transform.position) >= attackRange)
        {
            temp = Vector2.MoveTowards(transform.position, target.transform.position, dexterity);
            thisBody.MovePosition(temp);
            //AvoidEnvironment();
            anim.SetFloat("moveX", target.transform.position.x - transform.position.x );//allows movement animation
            anim.SetBool("moving", true);//moving set true in animator
        }
        else if(Vector2.Distance(transform.position, target.transform.position) <= attackRange &&
            Vector2.Distance(transform.position, target.transform.position) >= backOffRange)
        {
            transform.RotateAround(target.transform.position, Vector3.forward, dexterity);
            //AvoidEnvironment();
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            anim.SetFloat("moveX", target.transform.position.x - transform.position.x);//allows movement animation
            anim.SetBool("moving", true);//moving set true in animator
        }
        else if(Vector2.Distance(transform.position, target.transform.position) <= backOffRange)
        {            
            temp = Vector2.MoveTowards(transform.position,target.transform.position * -1, dexterity);
            thisBody.MovePosition(temp);
            //AvoidEnvironment();
        }
        else
        {
            temp = Vector2.MoveTowards(transform.position, homePosition, dexterity / 1.5f);
            thisBody.MovePosition(temp);
            //AvoidEnvironment();
            anim.SetFloat("moveX", homePosition.x - target.transform.position.x);//allows movement animation
            castV3 = transform.position;
            if(castV3 == homePosition)
            {
                anim.SetBool("moving", false);
                //currentState = EnemyState.idle;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Collision")
        {
            if(this.transform.position.x - collision.transform.position.x < 0)
            {
                Debug.Log("asl;dfkj");
            }
        }
    }
}
