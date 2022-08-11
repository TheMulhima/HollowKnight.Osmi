using Osmi.ChildRefs;
using Osmi.Game;

namespace Osmi.SimpleFSM;

public abstract partial class SimpleFSM : MonoBehaviour {
	protected void Awake() {
		Type = GetType();
		Name = Type.Name;
		info = new(() => SimpleFSMInfo.map[Type]);
		existingFSMs.Add(this);

		if (InitChildRefsOnAwake) {
			this.InitChildRefs();
		}
	}

	protected void Start() {
		if (!InitChildRefsOnAwake) {
			this.InitChildRefs();
		}

		SetState(nameof(Init));
	}

	protected void OnEnable() {
		if (RestartOnEnable) {
			SetState(nameof(Init));
		}

		activeFSMs.Add(this);
		State = SimpleFSMState.Running;
	}

	protected void OnDisable() {
		State = SimpleFSMState.Disabled;

		if (waitCoro != null) {
			GlobalCoroutineExecutor.Stop(waitCoro);
			waitCoro = null;
		}

		_ = activeFSMs.Remove(this);
	}

	protected void OnDestroy() =>
		existingFSMs.Remove(this);

	protected void Update() => Iterate();
}
