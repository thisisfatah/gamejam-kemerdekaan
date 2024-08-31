using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZDepth : MonoBehaviour
{
	private void LateUpdate()
	{
		Vector3 pos = transform.position;
		pos.z = pos.y * 0.0003f;
		transform.position = pos;
	}
}
