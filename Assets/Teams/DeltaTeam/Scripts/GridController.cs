using System;
using System.Collections;
using System.Collections.Generic;
using DeltaTeam;
using DoNotModify;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField;
	public GridDebug gridDebug;
	public GameData gameData;

	public Vector2Int gridOffset;
	

	private void Start()
	{
		gameData = GameManager.Instance.GetGameData();
	}


	private void InitializeFlowField()
	{
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.gridOffset = gridOffset;
        curFlowField.CreateGrid();
		gridDebug.SetFlowField(curFlowField);
	}
    

	private void Update()
	{
		
		if (gameData == null && GameManager.Instance.GetGameData() != null)
		{
			gameData = GameManager.Instance.GetGameData();
		}

		if (Input.GetKey(KeyCode.Q))
		{
			InitializeFlowField();

			curFlowField.CreateCostField();

			Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
			Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
			Cell destinationCell = curFlowField.GetCellFromWorldPos(gameData.SpaceShips[0].Position);
			curFlowField.CreateIntegrationField(destinationCell);

			curFlowField.CreateFlowField();

			gridDebug.DrawFlowField();
		}
	}

	public void CreateNewFlowField(Vector2 targetPos)
	{
		if (curFlowField != null)
		{
			if (curFlowField.destinationCell != null && (Vector2)curFlowField.destinationCell.worldPos == targetPos)
			{
				return;
			}
		}
		InitializeFlowField();
		curFlowField.CreateCostField();
		Cell destinationCell = curFlowField.GetCellFromWorldPos(targetPos);
		curFlowField.CreateIntegrationField(destinationCell);
		curFlowField.CreateFlowField();
		gridDebug.DrawFlowField();
	}
}
