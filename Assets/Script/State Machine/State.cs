using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State//The basic states of the AI
{   //Week 2 Finate state Machines - https://moodle.bcu.ac.uk/course/view.php?id=79511
    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();
}
