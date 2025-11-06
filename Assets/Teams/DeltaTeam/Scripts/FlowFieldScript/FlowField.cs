using System;
using System.Collections;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

public class FlowField
{
	public Cell[,] grid { get; private set; }
	public Vector2Int gridSize { get; private set; }
	public float cellRadius { get; private set; }
	public Cell destinationCell;

	private float cellDiameter;
	
	public Vector2Int gridOffset;

	private List<AsteroidView> listOfAsteroid;

	public FlowField(float _cellRadius, Vector2Int _gridSize)
	{
		cellRadius = _cellRadius;
		cellDiameter = cellRadius * 2f;
		gridSize = _gridSize;
	}

	public void CreateGrid()
	{
		listOfAsteroid = GameManager.Instance.GetGameData().Asteroids;
		grid = new Cell[gridSize.x, gridSize.y];

		for (int x = 0; x < gridSize.x; x++)
		{
			for (int y = 0; y < gridSize.y; y++)
			{
				Vector3 worldPos = new Vector3(cellDiameter * x + cellRadius + gridOffset.x, 
					cellDiameter * y + cellRadius + gridOffset.y, 0);
				grid[x, y] = new Cell(worldPos, new Vector2Int(x, y));
			}
		}
	}

	//Deprecated for optimisation
	// public void CreateCostField()
	// {
	// 	Vector3 cellHalfExtents = Vector3.one * cellRadius;
	// 	cellHalfExtents.z = 0;
	// 	int terrainMask = LayerMask.GetMask("Asteroid");
	// 	foreach (Cell curCell in grid)
	// 	{
	// 		if (listOfAsteroid.Count != 0)
	// 		{
	// 			foreach (var asteroid in listOfAsteroid)
	// 			{
	// 				float asteroidRadius = asteroid.Radius + 0.5f;
	// 				Vector2 center = asteroid.Position;
	// 				Cell asteroidCellCenter = GetCellFromWorldPos(center);
	// 				if (Vector2.Distance(center, (Vector2)curCell.worldPos) <= asteroidRadius - 0.2f)
	// 				{
	// 					curCell.IncreaseCost(255);
	// 				}
	// 				if (Vector2.Distance(center, (Vector2)curCell.worldPos) <= asteroidRadius + 0.5f)
	// 				{
	// 					curCell.IncreaseCost(3);
	// 				}
	// 			}
	// 		}
	// 	}
	// }
	
	
	public void CreateCostField()
	{
		listOfAsteroid = GameManager.Instance.GetGameData().Asteroids;

		foreach (var asteroid in listOfAsteroid)
		{
			float asteroidRadius = asteroid.Radius + 0.5f;
			Vector2 center = asteroid.Position;

			// calculer la zone à vérifier dans la grille
			int minX = Mathf.Max(0, Mathf.FloorToInt((center.x - asteroidRadius - gridOffset.x) / cellDiameter));
			int maxX = Mathf.Min(gridSize.x - 1, Mathf.CeilToInt((center.x + asteroidRadius - gridOffset.x) / cellDiameter));
			int minY = Mathf.Max(0, Mathf.FloorToInt((center.y - asteroidRadius - gridOffset.y) / cellDiameter));
			int maxY = Mathf.Min(gridSize.y - 1, Mathf.CeilToInt((center.y + asteroidRadius - gridOffset.y) / cellDiameter));

			for (int x = minX; x <= maxX; x++)
			{
				for (int y = minY; y <= maxY; y++)
				{
					Cell curCell = grid[x, y];
					float dist = Vector2.Distance(center, (Vector2)curCell.worldPos);

					if (dist <= asteroidRadius - 0.2f)
						curCell.IncreaseCost(255);
					else if (dist <= asteroidRadius + 0.5f)
						curCell.IncreaseCost(3);
				}
			}
		}
	}


	public void CreateIntegrationField(Cell _destinationCell)
	{
		destinationCell = _destinationCell;

		destinationCell.cost = 0;
		destinationCell.bestCost = 0;

		Queue<Cell> cellsToCheck = new Queue<Cell>();

		cellsToCheck.Enqueue(destinationCell);

		while(cellsToCheck.Count > 0)
		{
			Cell curCell = cellsToCheck.Dequeue();
			List<Cell> curNeighbors = GetNeighborCells(curCell.gridIndex, GridDirection.CardinalDirections);
			foreach (Cell curNeighbor in curNeighbors)
			{
				if (curNeighbor.cost == byte.MaxValue) { continue; }
				if (curNeighbor.cost + curCell.bestCost < curNeighbor.bestCost)
				{
					curNeighbor.bestCost = (ushort)(curNeighbor.cost + curCell.bestCost);
					cellsToCheck.Enqueue(curNeighbor);
				}
			}
		}
	}

	public void CreateFlowField()
	{
		foreach(Cell curCell in grid)
		{
			List<Cell> curNeighbors = GetNeighborCells(curCell.gridIndex, GridDirection.AllDirections);

			int bestCost = curCell.bestCost;

			foreach(Cell curNeighbor in curNeighbors)
			{
				if(curNeighbor.bestCost < bestCost)
				{
					bestCost = curNeighbor.bestCost;
					curCell.bestDirection = GridDirection.GetDirectionFromV2I(curNeighbor.gridIndex - curCell.gridIndex);
				}
			}
		}
	}

	private List<Cell> GetNeighborCells(Vector2Int nodeIndex, List<GridDirection> directions)
	{
		List<Cell> neighborCells = new List<Cell>();

		foreach (Vector2Int curDirection in directions)
		{
			Cell newNeighbor = GetCellAtRelativePos(nodeIndex, curDirection);
			if (newNeighbor != null)
			{
				neighborCells.Add(newNeighbor);
			}
		}
		return neighborCells;
	}

	private Cell GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
	{
		Vector2Int finalPos = orignPos + relativePos;

		if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
		{
			return null;
		}

		else { return grid[finalPos.x, finalPos.y]; }
	}

	public Cell GetCellFromWorldPos(Vector3 worldPos)
	{
		Vector2 offset = gridOffset;

		float percentX = (worldPos.x - offset.x) / (gridSize.x * cellDiameter);
		float percentY = (worldPos.y - offset.y) / (gridSize.y * cellDiameter);

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.Clamp(Mathf.FloorToInt(gridSize.x * percentX), 0, gridSize.x - 1);
		int y = Mathf.Clamp(Mathf.FloorToInt(gridSize.y * percentY), 0, gridSize.y - 1);

		return grid[x, y];
	}

}
