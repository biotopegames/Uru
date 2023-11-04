using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Rune", menuName = "Item/Rune")]
public class Rune : Item
{
    public enum RuneType
    {
        DefenseRune,
        OffenseRune,
        CreatureRune
    }

    public RuneType runeType;
    public int hp;
    public int attack;
}