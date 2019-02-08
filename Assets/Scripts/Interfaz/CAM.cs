using UnityEngine;
using System.Collections;
// Adaptador de camara respecto a la dimension del dispositivo
public class CAM : MonoBehaviour {

	public float orthographicSize = 2; // Que tan cerca esta
	public float aspect = 1.33333f; // Relacion ancho-altura

	void FixedUpdate () {
		Camera.main.projectionMatrix = Matrix4x4.Ortho(
			-orthographicSize * aspect, orthographicSize * aspect,
						-orthographicSize, orthographicSize,Camera.main.nearClipPlane, Camera.main.farClipPlane);
	}
}
