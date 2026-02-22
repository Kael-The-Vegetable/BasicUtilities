using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
	public Text scoreText;
	public void OnEnable()
	{
		GameManager.Instance.OnScoreChanged.AddListener(OnScoreChanged);
		scoreText.text = $"Current Score: {GameManager.Instance.Score}";
	}

	public void OnDisable()
	{
		if (GameManager.HasInstance)
		{
			GameManager.Instance.OnScoreChanged.RemoveListener(OnScoreChanged);
		}
	}

	private void OnScoreChanged(int newScore)
	{
		scoreText.text = $"Current Score: {newScore}";
	}
}
