using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
	public Vector2Int Size = Vector2Int.one;

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
}
