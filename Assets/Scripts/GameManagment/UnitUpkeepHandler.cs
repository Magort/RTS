using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitUpkeepHandler : MonoBehaviour
{
    public static UnitUpkeepHandler Instance;
    public List<UpkeepData> upkeeps = new();
    WaitForSeconds waiter = new(1);

    int patience = 0;
    int maxPatience = 5;

    [System.Serializable]
    public class UpkeepData
    {
        public UpkeepData(float value, Resource resource, Coroutine coroutine)
        {
            this.value = value;
            this.resource = resource;
            this.coroutine = coroutine;
        }
        public float value;
        public Resource resource;
        public Coroutine coroutine;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void AddUpkeep(float value, Resource resource)
    {
        upkeeps.Add(new(value, resource, StartCoroutine(Upkeep(value, resource))));
        GameState.AddResourceGrowth(resource, -value);
	}
    public void RemoveUpkeep(float value, Resource resource)
    {
        var toRemove = upkeeps.First(upkeep => upkeep.value == value && upkeep.resource == resource);
        StopCoroutine(toRemove.coroutine);
		GameState.AddResourceGrowth(resource, value);
		upkeeps.Remove(toRemove);
    }

    IEnumerator Upkeep(float value, Resource resource)
    {
        float accumulatedAmount = 0;

        while (true)
        {
            yield return waiter;

            accumulatedAmount += value;

            if (accumulatedAmount <= GameState.Resources[resource])
                GameState.AddResource(resource, -Mathf.FloorToInt(accumulatedAmount));
            else
                CheckPatience();

			accumulatedAmount -= Mathf.FloorToInt(accumulatedAmount);
        }
    }

    void CheckPatience()
    {
        patience++;

        if (patience < maxPatience)
            return;

        RemoveRandomUnit();
        patience = 0;
    }

    void RemoveRandomUnit()
    {
        var tilesWithPlayerUnits = TileGrid.Tiles
            .Where(tile => tile.data.units.Count > 0)
            .Where(tile => tile.data.units.Find(unit => unit.affiliation == Affiliation.Player) != null)
            .ToList();

        var randomTile = tilesWithPlayerUnits[Random.Range(0, tilesWithPlayerUnits.Count)];
        var randomMapUnit = randomTile.data.units[Random.Range(0, randomTile.data.units.Count)];

		randomMapUnit.KillRandomUnit();
    }
}
