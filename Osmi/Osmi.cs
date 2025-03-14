using Osmi.Exports;
using Osmi.Game;

namespace Osmi;

[PublicAPI]
public sealed class Osmi : Mod {
	public static Osmi? Instance { get; private set; }

	public static Osmi UnsafeInstance => Instance!;

	public static Lazy<string> version = AssemblyUtil
#if DEBUG
		.GetMyDefaultVersionWithHash();
#else
		.GetMyDefaultVersion();
#endif

	public override string GetVersion() => version.Value;

	public Osmi() =>
		Instance = this;

	public override void Initialize() {
		_ = GlobalCoroutineExecutor.Start(OsmiHooks.CheckGameInitialized());

		ModInteropExports.Init();
	}

	public static bool IsHere() => true;
}
