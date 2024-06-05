using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBehaviourHandler : MonoBehaviour
{
	public static EnemyBehaviourHandler Instance;
	public List<MapUnit> wildUnits;

	public int gracePeriod;
	[Header("Move")]
	public int minMovePeriod;
	public int maxMovePeriod;
	[Range(0.1f, 0.5f)] public float amountOfUnitsToMove;
	public int maxStep;
	[Header("Spawn")]
	public int spawnPeriod;
	public List<MapGenerator.NeutralMapUnitSpawnData> spawns;

	float threatLevel = 0;
	float maxThreat = 1;
	WaitForSeconds oneSecondWaiter = new(1);

	List<MapUnit> recentlyMoved = new();

	private void Awake()
	{
		Instance = this;
		StartCoroutine(GracePeriod());
	}

	public void AddUnit(MapUnit unit)
	{
		wildUnits.Add(unit);
	}

	public void RemoveUnit(MapUnit unit)
	{
		wildUnits.Remove(unit);
	}

	IEnumerator GracePeriod()
	{
		float timer = 0;

		while (timer < gracePeriod)
		{
			yield return oneSecondWaiter;
			timer++;
		}

		StartCoroutine(MoveWildUnits());
		StartCoroutine(SpawnEnemies());
	}

	IEnumerator MoveWildUnits()
	{
		while (wildUnits.Count > 0)
		{
			List<MapUnit> unitsToMove = wildUnits.Except(recentlyMoved).ToList();
			int amountToMove = Mathf.RoundToInt(unitsToMove.Count * amountOfUnitsToMove);
			List<MapUnit> unitsMoved = new();


			for (int i = 0; i < amountToMove; i++)
			{
				var roll = Random.Range(0, unitsToMove.Count);
				UnitMovementHandler.Instance.MoveEnemyUnit(unitsToMove[roll], maxStep);
				unitsMoved.Add(unitsToMove[roll]);
				unitsToMove.Remove(unitsToMove[roll]);
			}

			recentlyMoved = unitsMoved;

			yield return new WaitForSeconds(Random.Range(minMovePeriod, minMovePeriod));
		}
	}

	IEnumerator SpawnEnemies()
	{
		WaitForSeconds spawnWaiter = new(spawnPeriod);

		while (TileGrid.WinTargetTiles.Count > 0)
		{
			foreach (Tile tile in TileGrid.WinTargetTiles)
			{
				var unit = spawns.First(setup => setup.tier == threatLevel).units[Random.Range(0, 2)];

				tile.AddUnit(unit);
				wildUnits.Add(unit);
				UnitMovementHandler.Instance.MoveEnemyUnit(unit, 25);
			}

			if (threatLevel < maxThreat)
				threatLevel++;

			yield return spawnWaiter;
		}
	}

}
