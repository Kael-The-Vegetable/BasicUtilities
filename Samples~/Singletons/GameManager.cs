using BasicUtilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class GameManager : AnnexSingleton<GameManager>
{
	private int _score = 0;
	public int Score
	{
		get => _score;
		set
		{
			if (_score != value)
			{
				_score = value;
				OnScoreChanged.Invoke(_score);
			}
		}
	}
	public UnityEvent<int> OnScoreChanged { get; } = new();
	protected override void Initialize() {}

	public void AddScore(int amount)
	{
		Score += amount;
	}
	public void SaveScore()
	{
		HighscoreManager.Instance.AddScore(Score);
		SceneLoader.Instance.LoadScene("ScoreScene");
	}
}
