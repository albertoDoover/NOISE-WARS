using UnityEngine;
using System.Collections;

public class CAM : MonoBehaviour {

	public float orthographicSize = 2;
	public float aspect = 1.33333f;

	void FixedUpdate () {
		Camera.main.projectionMatrix = Matrix4x4.Ortho(
			-orthographicSize * aspect, orthographicSize * aspect,
						-orthographicSize, orthographicSize,Camera.main.nearClipPlane, Camera.main.farClipPlane);
	}
}
