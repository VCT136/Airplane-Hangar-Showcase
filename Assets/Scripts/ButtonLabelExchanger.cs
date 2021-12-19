using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * written by Vincent Busch
 * 
 * This script exchanges the text (TextMeshPro) on a UI button with another one and back whenever the exchange function is called.
 */
public class ButtonLabelExchanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private string textA;
    [SerializeField] private string textB;

    public void Exchange() {
        if (labelText.text == textB) labelText.text = textA;
        else labelText.text = textB;
	}
}
