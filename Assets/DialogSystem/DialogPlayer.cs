using DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class DialogPlayer : MonoBehaviour, IDialogContext
{
    //TODO
    [SerializeField] TMP_Text _speaker;

    [SerializeField] TMP_Text _speech;

    [SerializeField] Story _story;
    [SerializeField] List<GameObject> _variantButtons;
    NodeBase _currentNode;
    int _selectedVariant = 0;
    bool DialogActive => true;
    int VariantsCount => _currentNode.GetVariants().Length;
    Dictionary<string, NodeBase> _nodesDictionary = new();
    private void Start()
    {
        foreach (var node in _story.nodes)
        {
            if (_nodesDictionary.ContainsKey(node.id) == false)
            {
                _nodesDictionary.Add(node.id, node);
            }
        }
    }
    private void Update()
    {
        if (DialogActive)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (VariantsCount == 0)
                {
                    ProcessNextNode();

                }
            }
        }
    }
    [ContextMenu(nameof(StartDialog))]
    public void StartDialog()
    {
        _currentNode = _story.nodes[0];
        ProcessNode();
    }
    public void ProcessNextNode()
    {
        _currentNode.OnExit();
        _currentNode = _nodesDictionary[_currentNode.GetNext()];

        ProcessNode();
    }
    public void ProcessNode()
    {
        ClearButtons();
        if (_currentNode.exitNode)
        {
            _speech.text = string.Empty;
            return;
        }
        NodeBase node = _currentNode;
        node.Init(this);
        node.OnEnter();
        _speech.text = node.GetSpeech();
        Variant[] variants = node.GetVariants();
        ActivateNodeButtons(variants);
    }
    void ActivateNodeButtons(Variant[] variants)
    {
        for (int i = 0; i < variants.Length; i++)
        {
            _variantButtons[i].SetActive(true);
            _variantButtons[i].GetComponentInChildren<TMP_Text>().text = variants[i].text;
        }
    }
    void ClearButtons()
    {
        foreach (var item in _variantButtons)
        {
            item.SetActive(false);
        }
    }
    public object GetValue(string id)
    {
        VariableConditionInterpretator interpretator = new();
        return interpretator.Execute(id);
    }
    public void SelectVariant(int num)
    {
        _currentNode.OnExit();
        _selectedVariant = num;
        _currentNode = _nodesDictionary[_currentNode.GetVariants()[num].nextId];
        ProcessNode();
    }
}