using DialogSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[CreateAssetMenu(menuName = "Dialog/ConditionOnEnterNode")]

public class ConditionOnEnterNode : Node
{
    public string conditionFailedSpeech;

    public VariantCondition beforeEnterCondition;
    public UnityAction onPassedAction;
    public UnityAction onFailedAction;
    public string onPassNextId => idNext;
    public string onFailedNextId;


    bool _conditionCheckResult = false;

    public override string GetSpeech()
    {
        return _conditionCheckResult?speech:conditionFailedSpeech;
    }
    public override string GetNext()
    {
        return _conditionCheckResult ?onPassNextId:onFailedNextId;
    }
    public override void OnEnter()
    {
        _conditionCheckResult = beforeEnterCondition.Try(context);
        if (_conditionCheckResult == true)
        {
            onPassedAction?.Invoke();
        }
        else
        {
            onFailedAction?.Invoke();
        }
    }
    
}