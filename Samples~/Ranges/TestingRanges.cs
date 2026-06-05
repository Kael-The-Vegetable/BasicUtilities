using BasicUtilities;
using UnityEngine;
using UnityEngine.UI;

public class TestingRanges : MonoBehaviour
{
	public FloatRange range;
	public Button randButton;
	public Slider minSlider;
	public Slider maxSlider;
	public Text rangeText;
	public Text randText;

	public void Awake()
	{
		randButton.onClick.AddListener(Random);
		minSlider.onValueChanged.AddListener(value => SliderListener(maxSlider, value));
		maxSlider.onValueChanged.AddListener(value => SliderListener(minSlider, value));
		rangeText.text = range.ToString();
	}

	private void SliderListener(Slider sliderToChange, float value)
	{
		if (sliderToChange == minSlider)
		{
			range.Max = value;
			sliderToChange.maxValue = value;
		}
		else
		{
			range.Min = value;
			sliderToChange.minValue = value;
		}
		rangeText.text = range.ToString();
	}
	private void Random()
	{
		randText.text = range.Get().ToString();
	}
}
