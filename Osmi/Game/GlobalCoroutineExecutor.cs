namespace Osmi.Game;

[PublicAPI]
public sealed class GlobalCoroutineExecutor : MonoBehaviour {
	private static readonly GlobalCoroutineExecutor instance
		= GameObjectUtil.CreateHolder<GlobalCoroutineExecutor>("Global Coroutine Executor");

	public static Coroutine Start(IEnumerator enumerator) =>
		instance.StartCoroutine(enumerator);

	public static void Stop(IEnumerator enumerator) =>
		instance.StopCoroutine(enumerator);

	public static void Stop(Coroutine coroutine) =>
		instance.StopCoroutine(coroutine);

	internal static void StopAll() =>
		instance.StopAllCoroutines();

	public static Coroutine SetTimeOut(float timeOut, Action action) => instance.SetTimeOut(timeOut, action);

	public static Coroutine SetImmediate(Action action) => instance.SetImmediate(action);
}
