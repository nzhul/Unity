using UnityEngine;
using System.Collections;
using System;	// added to access the enum class

public class BaseCharacter : MonoBehaviour {
	private string _name;
	private int _level;
	private uint _freeExp;
	
	private Attribute[] _primaryAttribute;
	private Vital[] _vital;
	private Skill[] _skill;
	
	public void Awake()
	{
		_name = string.Empty;
		_level = 0;
		_freeExp = 0;
		
		_primaryAttribute = new Attribute[Enum.GetValues(typeof(AttributeName)).Length];
		_vital = new Vital[Enum.GetValues(typeof(VitalName)).Length];
		_skill = new Skill[Enum.GetValues(typeof(SkillName)).Length];
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public string Name
	{
		get{return _name;}
		set{_name = value;}
	}
	public int Level
	{
		get{return _level;}
		set{_level = value;}
	}
	public uint FreeExp
	{
		get{return _freeExp;}
		set{_freeExp = value;}
	}
	
	public void AddExp(uint exp)
	{
		_freeExp += exp;
		CalculateLevel();
	}
	
	// take avg of all of the players skills and assign that as the player level.
	public void CalculateLevel()
	{
		
	}
	
	private void SetupPrimaryAttribute()
	{
		for (int cnt = 0; cnt < _primaryAttribute.Length; cnt++) {
			_primaryAttribute[cnt] = new Attribute();
		}
	}
	private void SetupVitals()
	{
		for (int cnt = 0; cnt < _vital.Length; cnt++) {
			_primaryAttribute[cnt] = new Attribute();
		}
	}
	private void SetupSkills()
	{
		for (int cnt = 0; cnt < _skill.Length; cnt++) {
			_primaryAttribute[cnt] = new Attribute();
		}
	}
}
