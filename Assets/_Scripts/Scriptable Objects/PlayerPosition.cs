using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Position", menuName = "ScriptableObjects/Player Position")]
public class PlayerPosition : ScriptableObject
{
    public float x;
    public float y;
}