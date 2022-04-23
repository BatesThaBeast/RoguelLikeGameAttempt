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
    void Push();
}



public interface IDataPersistence//for loading or saving data
{
    void LoadData(GameData data);
    void SaveData(GameData data);
}

