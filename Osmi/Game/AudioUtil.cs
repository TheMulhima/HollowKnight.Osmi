namespace Osmi.Game;

[PublicAPI]
public static class AudioUtil {
	public static AudioSource PlayOneShot(GameObject playerPrefab, Vector3 position, AudioClip clip, float pitchMin, float pitchMax, float volumn) =>
		PlayOneShot(playerPrefab, position, clip, UnityEngine.Random.Range(pitchMin, pitchMax), volumn);

	public static AudioSource PlayOneShot(GameObject playerPrefab, Vector3 position, AudioClip clip, float pitch, float volumn) {
		AudioSource source = playerPrefab.Spawn(position, Quaternion.Euler(Vector3.up)).GetComponent<AudioSource>();
		source.pitch = pitch;
		source.volume = volumn;
		source.PlayOneShot(clip);
		return source;
	}

	public static AudioSource PlayOneShot(GameObject playerPrefab, Vector3 position, AudioClip clip, float pitch) =>
		PlayOneShot(playerPrefab, position, clip, pitch, 1f);
}
