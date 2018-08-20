using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerState{
	Initializing,
	Started,
	Error,
	Shutdown
}

public interface IManager{
	ManagerState State {get;}
	void Startup();
}
