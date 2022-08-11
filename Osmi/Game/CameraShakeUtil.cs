namespace Osmi.Game;

public static class CameraShakeUtil {
	public static PlayMakerFSM cameraShakeFsm => Ref.GC.cameraShakeFSM;

	public static void Small() => cameraShakeFsm.SendEvent("SmallShake");
	public static void Big() => cameraShakeFsm.SendEvent("BigShake");
	public static void Huge() => cameraShakeFsm.SendEvent("HugeShake");
	public static void SuperDash() => cameraShakeFsm.SendEvent("SuperDashShake");
	public static void EnemyKill() => cameraShakeFsm.SendEvent("EnemyKillShake");
	public static void Tram() => cameraShakeFsm.SendEvent("TramShake");
	public static void Average() => cameraShakeFsm.SendEvent("AverageShake");
	public static void Blizzard() => cameraShakeFsm.SendEvent("BlizzardShake");

	public static void Cancel() => cameraShakeFsm.SendEvent("CANCEL SHAKE");
}
