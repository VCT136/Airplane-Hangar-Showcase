using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * written by Vincent Busch
 * 
 * This script is used to change the position of a UI element using an anchored rect transform.
 */
public class UIRepositioner : MonoBehaviour
{
	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private Vector2 offset;

	// reposition the element according to the offset
	public void Offset() {
		rectTransform.anchoredPosition += offset;
	}

	// opposite of the offset function
	public void AntiOffset() {
		rectTransform.anchoredPosition -= offset;
	}

	// offset and then reverse this script's offset field
	public void OffsetAndReverse() {
		Offset();
		offset *= -1;
	}
}
