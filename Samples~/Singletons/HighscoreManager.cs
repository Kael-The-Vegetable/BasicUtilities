using BasicUtilities;
using UnityEngine;

public class HighscoreManager : PersistentSingleton<HighscoreManager>
{
	private int[] _scores = new int[10];
	public int[] Scores => _scores;

	protected override void Initialize() {}

	
	public void AddScore(int score)
	{
		for (int i = 0; i < _scores.Length; i++)
		{
			if (score > _scores[i])
			{
				for (int j = _scores.Length - 1; j > i; j--)
				{
					_scores[j] = _scores[j - 1];
				}
				_scores[i] = score;
				break;
			}
		}
	}
	public void ClearScores()
	{
		for (int i = 0; i < _scores.Length; i++)
		{
			_scores[i] = 0;
		}
	}
}
