using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerState{
	Started,
	Initialized,
	Shutdown
}

public interface IManager{
	ManagerState State {get;}
	void Startup();
}
