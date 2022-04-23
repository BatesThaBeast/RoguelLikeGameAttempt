using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
   walk, attack, interact, stagger, dead, holdingBox, holdingBow
}

public class Player : Character , IDataPersistence , IPushable
{
    protected Animation weapon;
    protected PlayerState currentState = PlayerState.walk;//---------State Machine---------
    GameObject[] inRange;//---------Array to revive enemies---------
    void Start()
    {
        this.dexterity = 06f;
        this.weapon = GameObject.Find("BigSword").GetComponent<Animation>();
        this.push = 10f;
        this.pushTime = .3f;
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
    public float push { get; private set; }
    public float pushTime { get; private set; }
    public void Push(Collider2D obj)
    {
        Rigidbody2D character = obj.GetComponent<Rigidbody2D>();//reference to the Rigidbody component
        if (character != null)//makes sure the object hasn't already been destroyed
        {
            Vector2 difference = character.transform.position - transform.position;//not sure lol
            difference = difference.normalized * push;//this is where the thrust will change how far something is pushed back
            character.AddForce(difference, ForceMode2D.Impulse);//the actual push occurs here
            StartCoroutine(PushCo(character));
        }
    }
    public IEnumerator PushCo(Rigidbody2D character)//Author Johnathan Bates
    {
        if (character != null)
        {
            yield return new WaitForSeconds(pushTime);
            character.velocity = Vector2.zero;
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
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if ((this.CompareTag("Player") && obj.CompareTag("Enemy")) || (obj.CompareTag("Player") && this.CompareTag("Enemy")))//check to make sure either player hits enemy or enemy hits player
        {
            if (obj.gameObject != null)
            {
                //Damage(attackDamage, obj);
                Push(obj);
            }
        }
    }
}
