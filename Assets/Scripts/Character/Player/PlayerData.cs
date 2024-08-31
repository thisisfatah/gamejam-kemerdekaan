using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Player Data", menuName="Player/Data")]
public class PlayerData : ScriptableObject
{
    public Sprite characterSprite;
    public RuntimeAnimatorController animator;

    public bool useLight;
}
