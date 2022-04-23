using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
   walk, attack, interact, stagger, dead, holdingBox, holdingBow
}

public class Player : Character , IDataPersistence
{
    protected Animation weapon;
    protected PlayerState currentState = PlayerState.walk;//---------State Machine---------
    GameObject[] inRange;//---------Array to revive enemies---------
    void Start()
    {
        this.dexterity = 06f;
        this.weapon = GameObject.Find("BigSword").GetComponent<Animation>();
    }
    void Update()
    {
        GetAttackInput();
        getMoveInput();
    }    
    private void getMoveInput()
    {
        movement = Vector2.zero;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement != Vector2.zero)
        {
            Move(thisBody, movement, dexterity);
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }
    public void GetAttackInput()
    {
        if (Input.GetButtonDown("attack"))
        {
            if (currentState != PlayerState.attack)
            {
                currentState = PlayerState.attack;
                if (FaceMouse().x - transform.position.x > 0)
                { weapon.GetComponent<Animation>().Play("Sword_Swing_Left"); }
                else { weapon.GetComponent<Animation>().Play("Sword_Swing_Right"); }
                StartCoroutine(AttackCo());
            }
        }
    }
    private IEnumerator AttackCo()
    {
        yield return new WaitForSeconds(.6f);
        currentState = PlayerState.walk;
    }
    private Vector3 FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return mousePosition;        
    }
    //******************************************************************************************************************************************************
    //************************************************************DECLARING IMOVABLE************************************************************************
    //******************************************************************************************************************************************************
    public void Move(Rigidbody2D thisBody, Vector2 movement, float moveSpeed)//Author Johnathan Bates
    {
        if (thisBody.CompareTag("Player"))//checks to see if this is the player moving
        { 
            thisBody.MovePosition(thisBody.position + movement * moveSpeed * Time.fixedDeltaTime);//actually moves the player     
            anim.SetFloat("moveX", FaceMouse().x - transform.position.x);//allows movement animation
            anim.SetBool("moving", true);//moving set true in animator
        }        
    }
    //******************************************************************************************************************************************************
    //************************************************************DECLARING IDATAPERSISTENCE****************************************************************
    //******************************************************************************************************************************************************
    public void LoadData(GameData data)
    {
        this.currentHealth = data.currentHealth;
        if (this.currentHealth == 0)
        {
            this.currentHealth = 3;
        }
        this.transform.position = data.playerPosition;
        Debug.Log(data.playerPosition);
    }
    public void SaveData(GameData data)
    {
        data.currentHealth = this.currentHealth;
        data.playerPosition = this.transform.position;
        Debug.Log(data.playerPosition);
    }
}
