using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public string quiz;
    public Sprite illustratation;
    public bool hasTwoAnswers = false;
    public int answer1, answer2;
}
