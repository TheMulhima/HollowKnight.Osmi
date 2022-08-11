namespace Osmi.Utils;

[PublicAPI]
public static class DeconstructUtil {
	public static void Deconstruct(this Vector3 self, out float x, out float y, out float z) =>
		(x, y, z) = self.ToTuple();

	public static void Deconstruct(this Vector2 self, out float x, out float y) =>
		(x, y) = self.ToTuple();

	public static void Deconstruct(this RaycastHit2D hit, out Collider2D? collider) =>
		collider = hit.collider;

	public static void Deconstruct(this RaycastHit2D hit, out Vector2 point, out float distance) {
		point = hit.point;
		distance = hit.distance;
	}

	public static void Deconstruct(this RaycastHit2D hit, out Collider2D? collider, out Vector2 point, out float distance) {
		collider = hit.collider;
		point = hit.point;
		distance = hit.distance;
	}
}
