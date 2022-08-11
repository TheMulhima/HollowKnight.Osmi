using MonoMod.Utils;

using Osmi.Game;

namespace Osmi.SimpleFSM;

[PublicAPI]
public abstract partial class SimpleFSM {
	public Type Type { get; private set; } = null!;
	public string Name { get; private set; } = null!;

	public SimpleFSMInfo Info => info.Value;

	public SimpleFSMState State { get; private set; } = SimpleFSMState.Disabled;

	public string ActiveStateName { get; private set; } = "";

	public IReadOnlyDictionary<string, string> CurrentTransitions { get; private set; }
		= new Dictionary<string, string>();

	private Lazy<SimpleFSMInfo> info = null!;

	private IEnumerator? currentState;


	public void SendEvent(string eventName) {
		if (Info.GlobalTransitions.TryGetValue(eventName, out string stateName)) {
			SetState(stateName);
		} else if (CurrentTransitions.TryGetValue(eventName, out stateName)) {
			SetState(stateName);
		}
	}

	public void SendPMFSMEvent(FsmEvent fsmEvent) {
		if (AcceptPMFSMEvents) {
			SendEvent(fsmEvent.Name);
		}
	}

	public void SendEventDelayed(string eventName, float seconds) =>
		SendEventDelayed(eventName, new WaitForSeconds(seconds));

	public void SendEventDelayed(string eventName, YieldInstruction inst) =>
		GlobalCoroutineExecutor.Start(SendEventDelayedCoroutine(eventName, inst));

	public void SendEventDelayed(string eventName, IEnumerator enumerator) =>
		GlobalCoroutineExecutor.Start(SendEventDelayedCoroutine(eventName, enumerator));

	private IEnumerator SendEventDelayedCoroutine(string eventName, object obj) {
		yield return obj;
		SendEvent(eventName);
	}


	public void SetState(string stateName, params object[] args) {
		if (!Info.States.TryGetValue(stateName, out FastReflectionDelegate stateMethod)) {
			throw new ArgumentException($"Invalid state name {stateName}");
		}

		if (State == SimpleFSMState.Running || State == SimpleFSMState.Waiting) {
			State = SimpleFSMState.Suspended;
		}

		if (waitCoro != null) {
			GlobalCoroutineExecutor.Stop(waitCoro);
			waitCoro = null;
		}

		currentState = stateMethod.Invoke(this, args) as IEnumerator;
		CurrentTransitions = Info.Transitions[stateName];
		ActiveStateName = stateName;

		if (State == SimpleFSMState.Suspended) {
			State = SimpleFSMState.Running;
		}
	}


	protected void Iterate() {
		if (State != SimpleFSMState.Running) {
			return;
		}

		if (currentState == null || !currentState.MoveNext()) {
			State = SimpleFSMState.Suspended;
			currentState = null;
			SendEvent(FINISHED);
			return;
		}

		object? current = currentState.Current;
		if (current is string eventName) {
			SendEvent(eventName);
		} else if (current is YieldInstruction inst) {
			WaitFor(inst);
		} else if (current is IEnumerator enmerator) {
			WaitFor(enmerator);
		} else if (current is (string delayedEventName, YieldInstruction delayInst)) {
			SendEventDelayed(delayedEventName, delayInst);
		} else if (current is (string customDelayedEventName, IEnumerator customDelayEnumerator)) {
			SendEventDelayed(customDelayedEventName, customDelayEnumerator);
		}
	}

	protected void Iterate(int times) {
		if (times <= 0) {
			throw new ArgumentOutOfRangeException(nameof(times));
		}

		for (int i = 0; i < times; i++) {
			Iterate();
		}
	}


	#region Waiting

	private Coroutine? waitCoro;

	private void WaitFor(object inst) {
		State = SimpleFSMState.Waiting;
		waitCoro = GlobalCoroutineExecutor.Start(WaitCoroutine(inst));
	}

	private IEnumerator WaitCoroutine(object inst) {
		yield return inst;

		if (State == SimpleFSMState.Waiting) {
			State = SimpleFSMState.Running;
		}

		waitCoro = null;
	}

	#endregion
}
