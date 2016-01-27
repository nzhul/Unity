using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
	public class EnemyEventArgs : EventArgs
	{
		public GameObject TheEnemy { get; set; }

		public EnemyEventArgs(GameObject theEnemy)
		{
			TheEnemy = theEnemy;
		}
	}
}
