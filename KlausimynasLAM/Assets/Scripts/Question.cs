using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    public string themeName { get; set; }
    public string question { get; set; }
    public string opt1 { get; set; }
    public string opt2 { get; set; }
    public string opt3 { get; set; }
    public int correctOpt { get; set; }

    public string correctAnwserBonus { get; set; }
    public string wrongAnwserBonus { get; set; }
    public string picName { get; set; }

    public Question(string t, string q, string o1, string o2, string o3, int c, string cBonus, string wBonus, string pic)
    {
        themeName = t;
        question = q;
        opt1 = o1;
        opt2 = o2;
        opt3 = o3;
        correctOpt = c;
        correctAnwserBonus = cBonus;
        wrongAnwserBonus = wBonus;
        picName = pic;
    }
}
