using UnityEngine;
public class WaypointControl : MonoBehaviour 
{
	static Color     linkColor     = Color.green;
	public Color     waypointColor = Color.blue;
	public float     radius        = 0.1F;
	public Transform next;
	
	public void OnDrawGizmos() 
	{
		Gizmos.color = waypointColor;
		Gizmos.DrawSphere(transform.position, radius);
		if (next != null) 
		{
			Gizmos.color = linkColor;
			Gizmos.DrawLine(transform.position, next.position);
		}
	}
}