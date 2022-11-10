using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelsGrid : MonoBehaviour
{
	public Vector2Int GridSize = new Vector2Int(10, 10);
	private Model[,] grid;
	private Model grabbedModel;
	private Camera currentCamera;
	private bool isPointerOnPanel = false;

	private void Awake()
	{
		grid = new Model[GridSize.x, GridSize.y];

		// For now there is only one camera.
		// Need to change currentCamera link when it's necessary
		currentCamera = Camera.main;
	}

	// Function for spawning a model
	public void StartPlacingBuilding(Model modelPrefab)
	{
		if (grabbedModel != null)
		{
			Destroy(grabbedModel.gameObject);
		}

		grabbedModel = Instantiate(modelPrefab);
	}

	public void SetPointerOnPanel()
	{
		isPointerOnPanel = true;
	}

	public void RemovePointerOnPanel()
	{
		isPointerOnPanel = false;
	}

	private void Update()
	{
		// Destroy model when right mouse button is clicked
		if (Input.GetMouseButtonDown(1))
		{
			if (grabbedModel != null)
			{
				Destroy(grabbedModel.gameObject);
				grabbedModel = null;
			}
		}
		// Sets position of grabbedModel equal to cursor's position on plane
		if (grabbedModel != null)
		{
			var groundPlane = new Plane(Vector3.up, Vector3.zero);
			var ray = currentCamera.ScreenPointToRay(Input.mousePosition);

			if (groundPlane.Raycast(ray, out float position))
			{
				Vector3 worldPosition = ray.GetPoint(position);

				int x = Mathf.RoundToInt(worldPosition.x);
				int y = Mathf.RoundToInt(worldPosition.z);

				// Checking if user can place the grabbedModel by mouse position
				bool isPlacingAvailable = true;

				if (x < 0 || x > GridSize.x - grabbedModel.Size.x) isPlacingAvailable = false;

				if (y < 0 || y > GridSize.y - grabbedModel.Size.y) isPlacingAvailable = false;

				if (isPlacingAvailable && IsPlaceTaken(x, y)) isPlacingAvailable = false;

				grabbedModel.transform.position = new Vector3(x, 0, y);
				grabbedModel.SetTransparent(isPlacingAvailable);

				if (isPlacingAvailable && !isPointerOnPanel && Input.GetMouseButtonDown(0))
				{
					PlaceGrabbedModel(x, y);

				}

			}
		}

		if (grabbedModel != null)
		{
			var groundPlane = new Plane(Vector3.up, Vector3.zero);
			var ray = currentCamera.ScreenPointToRay(Input.mousePosition);

			if (groundPlane.Raycast(ray, out float position))
			{
				Vector3 worldPosition = ray.GetPoint(position);

				int x = Mathf.RoundToInt(worldPosition.x);
				int y = Mathf.RoundToInt(worldPosition.z);
				// RemoveGrabbedModel(, );
			}
		}
	}

	// Function checking if place is taken by other model
	private bool IsPlaceTaken(int placeX, int placeY)
	{
		for (int x = 0; x < grabbedModel.Size.x; x++)
		{
			for (int y = 0; y < grabbedModel.Size.y; y++)
			{
				if (grid[placeX + x, placeY + y] != null) return true;
				Debug.Log(grid);
			}
		}

		return false;
	}

	// Function for placing GrabbedModel;
	private void PlaceGrabbedModel(int placeX, int placeY)
	{
		for (int x = 0; x < grabbedModel.Size.x; x++)
		{
			for (int y = 0; y < grabbedModel.Size.y; y++)
			{
				grid[placeX + x, placeY + y] = grabbedModel;
			}

		}

		grabbedModel.SetNormal();
		grabbedModel = null;
	}


	// private void RemoveModel(int placeX, int placeY)
	// {
	// 	for (int x = 0; x < hoveredModel.Size.x; x++)
	// 	{
	// 		for (int y = 0; y < hoveredModel.Size.y; y++)
	// 		{
	// 			grid[placeX + x, placeY + y] = null;
	// 		}
	// 	}

	// 	grabbedModel = null;
	// }
}
