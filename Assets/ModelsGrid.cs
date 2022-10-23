using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelsGrid : MonoBehaviour
{
	public Vector2Int GridSize = new Vector2Int(10, 10);
	private Model[,] grid;
	private Model grabbedModel;
	private Camera currentCamera;

	private void Awake()
	{
		grid = new Model[GridSize.x, GridSize.y];

		// For now there is only one camera.
		// Need to change currentCamera link when it's necessary
		currentCamera = Camera.main;
	}

	// Functions for spawning a model
	public void StartPlacingBuilding(Model modelPrefab)
	{
		if (grabbedModel != null)
		{
			Destroy(grabbedModel.gameObject);
		}

		grabbedModel = Instantiate(modelPrefab);
	}

	private void Update()
	{
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

				if (isPlacingAvailable && Input.GetMouseButtonDown(0))
				{
					PlaceGrabbedModel(x, y);
				}
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
}
