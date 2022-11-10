using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
	public Vector2Int Size = Vector2Int.one;
	private Renderer ModelRenderer;
	private Color modelColor;
	private Material[] modelMaterials;
	private float objectHeight;

	void Awake()
	{
		// Getting modelRenderer
		ModelRenderer = GetComponentsInChildren<Renderer>()[0];

		if (ModelRenderer != null)
		{
			modelColor = ModelRenderer.material.color;
			modelMaterials = ModelRenderer.materials;
		}

		// Correctly calculates position Y of the model to put it on the plane
		// !DON'T use SCALE on the child of visuals element (must be (1, 1, 1))
		Transform childTransform = this.gameObject.transform.GetChild(0).GetChild(0);
		GameObject childGameObject = childTransform.gameObject;

		float childColliderHeight = childGameObject.GetComponent<Collider>().bounds.size.y;

		Debug.Log(childColliderHeight);

		childTransform.position += Vector3.up * (childColliderHeight / 2);
	}
	public void SetTransparent(bool available)
	{
		if (available)
		{
			ChangeMaterialsColor(Color.green);
		}
		else
		{
			ChangeMaterialsColor(Color.red);
		}
	}

	public void SetNormal()
	{
		if (modelColor != null)
		{
			ChangeMaterialsColor(modelColor);
		}
		else
		{
			ChangeMaterialsColor(Color.white);
		}

	}

	// Function that draws Gizmos grid for each model
	public void OnDrawGizmosSelected()
	{
		for (int x = 0; x < Size.x; x++)
		{
			for (int y = 0; y < Size.y; y++)
			{
				if ((x + y) % 2 == 0)
				{
					Gizmos.color = new Color(0.88f, 0f, 1f, .3f);
				}
				else
				{
					Gizmos.color = new Color(1f, 0.68f, 0f, .3f);
				}

				Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1f, .1f, 1f));
			}
		}
	}

	public void ChangeMaterialsColor(Color color)
	{
		foreach (Material material in modelMaterials)
		{
			material.color = color;
		}
	}
}
