using DialogSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[CreateAssetMenu(menuName = "Dialog/Node")]

public class Node : NodeBase
{
    public UnityAction onEnter;
    public UnityAction onExit;

    public string speech;

    public Variant[] variants;
    public override void OnExit()
    {
        onExit?.Invoke();
    }
    public override void OnEnter()
    {
        onEnter?.Invoke();
    }
    public override string GetSpeech()
    {
        return speech;
    }
    public override Variant[] GetVariants()
    {
        List<Variant> temp = new();
        for (int i = 0; i < variants.Length; i++)
        {
            if (variants[i].haveCondition == true)
            {
                if (variants[i].condition.Try(context))
                {
                    temp.Add(variants[i]);
                }
            }
            else
            {
                temp.Add(variants[i]);
            }
        }
        return temp.ToArray();
    }
}