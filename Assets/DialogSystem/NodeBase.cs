using System;
using UnityEngine;

namespace DialogSystem
{
    [Serializable]
    public class NodeBase : ScriptableObject
    {
        public string id => name;
        [SerializeField] protected string idNext;
        protected IDialogContext context;
        public bool exitNode;
        public void Init(IDialogContext context)
        {
            this.context = context;
        }
        public virtual string GetSpeech()
        {
            return id;
        }
        public virtual Variant[] GetVariants()
        {
            return new Variant[0];
        }
        public virtual string GetNext()
        {
            return idNext;
        }
        public virtual void OnEnter()
        {

        }
        public virtual void OnExit()
        {

        }
    }
    [Serializable]
    public class Variant
    {
        public string text;
        public string nextId;
        public bool haveCondition;
        public VariantCondition condition;
    }
}