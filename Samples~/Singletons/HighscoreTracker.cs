using UnityEngine;
using UnityEngine.UI;

public class HighscoreTracker : MonoBehaviour
{
	public Text[] highscoreTexts;

	public void OnEnable()
	{
		for (int i = 0; i < highscoreTexts.Length; i++)
		{
			highscoreTexts[i].text = $"Highscore {i + 1}: {HighscoreManager.Instance.Scores[i]}";
		}
	}
}
