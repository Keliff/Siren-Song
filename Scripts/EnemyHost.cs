using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyHost : MonoBehaviour {

	public static EnemyHost instance;

	public List<Enemy> enemies;
	public List<int> allInits;
	public Dictionary< int, List<Enemy> > initEnemy;
	public GameObject moveTileHost;

	private void Awake()
	{
		if ( instance == null )
			instance = this;

		enemies = new List<Enemy>();
		allInits = new List<int>();
		initEnemy = new Dictionary< int, List<Enemy> >();

		moveTileHost = this.gameObject.transform.FindChild("MoveTileHost").gameObject;
	}
	public void OnAddChild( Enemy enemy )

	{

	}

	public void OnRemoveChild( Enemy enemy )
	{
		
	}

	public void EnemyTurn()
	{
		enemies = Globals.enemyList;

		foreach( Enemy enemy in enemies )
		{
			// Make sure every enemy finds their target, and their distance to their target
			enemy.target = enemy.FindPlayerTarget();

			// If the list of all initiatives doesn't yet contain this enemy's initiative
			if ( !this.allInits.Contains( enemy.initiative ) )
			{
				// Add it to the list
				this.allInits.Add( enemy.initiative );
				// Create a new list in the dictionary at that initiative
				this.initEnemy.Add( enemy.initiative, new List<Enemy>() );
			}
			// Add this particular enemy into the list at that initiative
			this.initEnemy[ enemy.initiative ].Add( enemy );
		}
		// Sort all the initiatives
		allInits.Sort();

		List<int> intList = new List<int>(initEnemy.Keys);

		// This loop sorts all the enemies by their distance to their Player target
//		foreach( KeyValuePair< int, List<Enemy> > entry in initEnemy )
		foreach( int i in intList )
		{
//			this.initEnemy[ entry.Key ] = this.initEnemy[ entry.Key ].OrderBy( x => x.distanceToTarget ).ToList();
//			test = this.initEnemy[ entry.Key ].OrderBy( x => x.distanceToTarget ).ToList();
//			entry.Value = test;
			this.initEnemy[ i ] = this.initEnemy[ i ].OrderBy( x => x.distanceToTarget ).ToList();
		}

		print("--> Start enemy loop");

//		// This loop goes in order from lowest to highest initiative
//		foreach( int init in allInits )
//		{
//			// For each initiative this loop goes through the list of enemies for that initiative
//			foreach( Enemy enemy in this.initEnemy[ init ] )
//			{
////				enemy.executing = true;
//
////				enemy.AI();
//				enemy.AI();
//
//
//				// And calls their AI
////				while( enemy.executing )
////					enemy.AI();
//			}
//		}

		StartCoroutine( EnemyLoop() );

		print("--> Done with Enemy Loop");

		// Once this loop is over, all the enemies AI have been called, and therefore it can be Player's turn again
//		Globals.Turn.characterTurn = Globals.Turn.PLAYER_TURN;
	}


	IEnumerator EnemyLoop(  )
	{
		// This loop goes in order from lowest to highest initiative
		foreach( int init in allInits )
		{
			// For each initiative this loop goes through the list of enemies for that initiative
			foreach( Enemy enemy in this.initEnemy[ init ] )
			{
				print("---> Before enemy call");

				enemy.AI();

				print("---> Before wait");

				yield return new WaitForSeconds( 3f );

				print("---> After wait");
			}
		}

		Globals.Turn.characterTurn = Globals.Turn.PLAYER_TURN;

		enemies = new List<Enemy>();
		allInits = new List<int>();
		initEnemy = new Dictionary< int, List<Enemy> >();
	}

}





























