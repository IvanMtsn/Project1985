using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatsSO", menuName = "ScriptableObjects/EnemyStats")]
public class EnemyStatsSO : ScriptableObject
{
    [HideInInspector] public float MaxHealth { get; } = 90;
}
