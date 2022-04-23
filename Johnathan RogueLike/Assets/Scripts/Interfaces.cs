using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICollectable
{
    void Collect();
}
public interface IDamageable
{
    void Damage();
}
public interface IInteractable
{
    void Interact();
}
public interface IKillable
{
    void Kill();
}
public interface IMovable
{
    void Move();
}
public interface IPushable
{
    float push { get; }
    float pushTime { get; }
    void Push(Collider2D obj);
    IEnumerator PushCo(Rigidbody2D character);
}



public interface IDataPersistence//for loading or saving data
{
    void LoadData(GameData data);
    void SaveData(GameData data);
}

