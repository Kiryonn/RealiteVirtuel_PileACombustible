using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelMeter : MonoBehaviour
{
    [SerializeField] private string prefix;
    [SerializeField] private string suffix;
	[SerializeField] private Slider slider;
	[SerializeField] private TMP_Text label;
	[SerializeField] private bool prefixSpace;
	[SerializeField] private bool suffixSpace;

	private void Start()
	{
		slider.onValueChanged.AddListener(delegate { UpdateLabel(); });
	}

	private void UpdateLabel()
	{
		label.text = prefix + (prefixSpace ? " " : "") + slider.value.ToString() + (suffixSpace ? " " : "") + suffix;
	}
}
