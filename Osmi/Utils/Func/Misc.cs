namespace Osmi.Utils;

[PublicAPI]
public static partial class FuncUtil {
	public static T Combine<T>(T a, T b) where T : Delegate =>
		(T) Delegate.Combine(a, b);
}
