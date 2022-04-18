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
    public string opt4 { get; set; }
    public int correctOpt { get; set; }
    public string picName { get; set; }

    public Question(string t, string q, string o1, string o2, string o3, string o4, int c, string pic)
    {
        themeName = t;
        question = q;
        opt1 = o1;
        opt2 = o2;
        opt3 = o3;
        opt4 = o4;
        correctOpt = c;
        picName = pic;
    }

    public override string ToString()
    {
        return string.Format($"{themeName},{question},{opt1},{opt2},{opt3},{opt4},{correctOpt},{picName}");
    }

    public string getThemeName()
    {
        return themeName;
    }

    public string getQuestion()
    {
        return question;
    }

    public string getOpt1()
    {
        return opt1;
    }

    public string getOpt2()
    {
        return opt2;
    }

    public string getOpt3()
    {
        return opt3;
    }

    public string getOpt4()
    {
        return opt4;
    }

    public int getCorrectOpt()
    {
        return correctOpt;
    }

    public string getPicName()
    {
        return picName;
    }
}
