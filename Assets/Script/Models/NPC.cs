using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC{
	public string Name {get; private set;}
	public int Age {get; private set;}

	public NPC(string name, int age){
		Name = name;
		Age = age;
	}
}
