using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person
{
    public string name { get; set; }
    public int correcqs { get; set; }
    public int allqs { get; set; }
    public string time { get; set; }

    public Person(string n, int qs, int aq, string t)
    {
        name = n;
        correcqs = qs;
        allqs = aq;
        time = t;
    }

    public string GetCorrectAndAll()
    {
        return correcqs+"/"+allqs;
    }
    public override string ToString()
    {
        return string.Format($"{name},{correcqs},{allqs},{time}");
    }
}
