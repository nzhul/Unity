using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour {

	public Dictionary<string, Ability> Abilities;

	public AbilityManager()
	{
		this.Abilities = new Dictionary<string, Ability>();
		Abilities.Add("Shoot", new Ability { Name = "Shoot", IsUnlocked = false });
		Abilities.Add("WallJump", new Ability { Name = "WallJump", IsUnlocked = false });
		Abilities.Add("DoubleJump", new Ability { Name = "DoubleJump", IsUnlocked = false });
	}
}

public class Ability
{
	public string Name { get; set; }
	public bool IsUnlocked { get; set; }
}
