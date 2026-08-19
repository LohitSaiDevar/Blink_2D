using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class TypeWriterEffect : MonoBehaviour
{
    [SerializeField] float typingSpeed;

    [TextArea]
    public string fullText;

    TMP_Text textComponent;

    void OnEnable()
    {
        textComponent = GetComponent<TMP_Text>();
        textComponent.text = string.Empty;
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
