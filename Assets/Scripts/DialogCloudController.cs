using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogCloudController : MonoBehaviour
{
    [SerializeField] float displayTime = 2f;
    [SerializeField] Transform container;
    [SerializeField] TMP_Text messagePrefab;
    
    public void Display(string message)
    {
        StartCoroutine(DisplayMessage(message));
    }
    IEnumerator DisplayMessage(string message)
    {
        AddTextToView(message);
        yield return new WaitForSeconds(displayTime);
        DestroyLastText();
    }
    void AddTextToView(string message)
    {
        TMP_Text newText = Instantiate(messagePrefab, container);
        newText.text = message;
        newText.transform.SetAsLastSibling();
    }
    void DestroyLastText()
    {
        if (container.childCount == 0)
        {
            return;
        }
        Destroy(container.GetChild(0).gameObject);
    }
}
