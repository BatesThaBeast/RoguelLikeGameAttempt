using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CharacterType//State machine for the type of character
{
    player, meleeEnemy, rangedEnemy, npc
}

public class Character : MonoBehaviour
{
    protected float currentHealth;//Current health of character
    protected float maxHealth;//Max health of character
    protected float strength;//How much damage a character can do
    protected float dexterity;//How fast a character is
    protected float constitution;//How much defense a character has
    protected float intelligence;//How much magic power a character has
    protected Vector2 movement;
    protected Rigidbody2D thisBody;//Will be used to initialize the Rigidbody2d
    protected Animator anim;//Will be used to initialize the Animator
    CharacterType charType;//For character class state machine, initialize in child class

    private void Awake()
    {
        this.thisBody = this.gameObject.GetComponent<Rigidbody2D>();
        this.anim = this.gameObject.GetComponent<Animator>();
    }
}
