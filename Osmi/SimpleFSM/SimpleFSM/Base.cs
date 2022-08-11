namespace Osmi.SimpleFSM;

public abstract partial class SimpleFSM {
	protected virtual bool AcceptPMFSMEvents => false;
	protected virtual bool RestartOnEnable => false;

	protected virtual bool InitChildRefsOnAwake => false;

	[FSMState]
	protected virtual IEnumerator Init() => NoOp();

	protected IEnumerator NoOp() {
		yield break;
	}
}
