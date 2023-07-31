using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GestionDates : MonoBehaviour
{
    private TMP_InputField inputField;

    public void EditDate(TextMeshProUGUI date)
    {
        inputField = date.transform.parent.GetChild(1).GetComponent<TMP_InputField>();
        inputField.gameObject.SetActive(true);
        date.transform.parent.GetChild(1).GetComponent<TMP_InputField>().text = date.text;
        date.gameObject.SetActive(false);
    }

    public void SaveDate(TextMeshProUGUI date)
    {
        string texte = date.transform.parent.GetChild(1).GetComponent<TMP_InputField>().text;
        date.text = texte;
        inputField = date.transform.parent.GetChild(1).GetComponent<TMP_InputField>();
        inputField.gameObject.SetActive(false);
        date.gameObject.SetActive(true);
    }
}
