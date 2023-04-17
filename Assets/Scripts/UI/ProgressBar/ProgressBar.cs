using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image Bar;

	public void UpdateHealthBar(float current, float max)
	{
		float ratio = current / max;
		Bar.rectTransform.localPosition = new Vector3(Bar.rectTransform.rect.width * ratio - Bar.rectTransform.rect.width, 0, 0);
	}
}
