using UnityEngine;
using BasicUtilities;
using UnityEngine.InputSystem;
public class SummonBalls : MonoBehaviour
{
	public Ball ballPrefab;
	public int maxBalls;
	private MonoObjectPool<Ball> _ballPool;

	private void Awake()
	{
		_ballPool = new MonoObjectPool<Ball>(ballPrefab, transform, maxBalls / 2, maxBalls, false);
	}

	private void Update()
	{
		if (Mouse.current.leftButton.wasPressedThisFrame)
		{
			Ball ball = _ballPool.Get();
			if (ball != null)
			{
				ball.transform.localPosition = Vector3.zero;
				ball.rb.AddForce(Random.onUnitSphere, ForceMode.Impulse);
			}
		}
	}
}
