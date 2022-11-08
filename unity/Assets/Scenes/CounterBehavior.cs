using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CounterBehavior : MonoBehaviour
{
    public TMP_Text m_text;

    private int score;
    private const string message = "Reps: ";


    void Start()
    {
        this.score = 0;
        this.m_text.text = message;
    }


    void Update()
    {
        this.m_text.text = message + this.score;
    }

    public void AddRep()
    {
        this.score++;
        Update();
    }
}
