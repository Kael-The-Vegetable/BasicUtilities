using BasicUtilities;
using UnityEngine;

public class Ball : MonoBehaviour, IPoolable<Ball>
{
	public MonoObjectPool<Ball> PoolAccess { get; set; }
	public Rigidbody rb;

	public void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	public void OnEnable()
	{
		rb.linearVelocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		OneShotTimer.Delay(2, Fade, destroyCancellationToken);
	}

	public void Fade()
	{
		PoolAccess.Release(this);
	}
}
