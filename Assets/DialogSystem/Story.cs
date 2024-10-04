using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem
{
    [CreateAssetMenu(menuName = "Dialog/Story")]
    public class Story : ScriptableObject
    {
        public List<NodeBase> nodes;
    }
    
}