using UnityEngine;
using System.Collections;

public class Capteurs : MonoBehaviour {
	public float Taille_Lasers;

	public RaycastHit hit_devant_gauche, hit_devant, hit_devant_droite;
	public float distance_devant_gauche, distance_devant, distance_devant_droite;

	private Vector3 origine, Devant_gauche, Devant, Devant_droite;
	private float rotation;

	// Use this for initialization
	void Start () {
		Taille_Lasers = 5.0f;


        origine = transform.position + Vector3.up * 0.2f;

        rotation = transform.rotation.eulerAngles.y;
		float angle = rotation / 180 * Mathf.PI;

        Devant_gauche = new Vector3 (origine.x - Taille_Lasers * Mathf.Sin (angle - Mathf.PI / 4), origine.y, origine.z - Taille_Lasers * Mathf.Cos (angle - Mathf.PI / 4));
        Devant = new Vector3 (origine.x - Taille_Lasers * Mathf.Sin (angle), origine.y, origine.z - Taille_Lasers * Mathf.Cos (angle));
        Devant_droite = new Vector3 (origine.x - Taille_Lasers * Mathf.Sin (angle + Mathf.PI / 4), origine.y, origine.z - Taille_Lasers * Mathf.Cos (angle + Mathf.PI / 4));

        distance_devant_gauche = 0.0f;
        distance_devant = 0.0f;
        distance_devant_droite = 0.0f;


	}
	
	// Update is called once per frame
	void Update () {
        origine = transform.position + Vector3.up * 0.2f;

        rotation = transform.rotation.eulerAngles.y;
		float angle = rotation / 180 * Mathf.PI;

        Devant_gauche = new Vector3 (origine.x - Taille_Lasers * Mathf.Sin (angle - Mathf.PI / 4), origine.y, origine.z - Taille_Lasers * Mathf.Cos (angle - Mathf.PI / 4));
        Devant = new Vector3 (origine.x - Taille_Lasers * Mathf.Sin (angle), origine.y, origine.z - Taille_Lasers * Mathf.Cos (angle));
        Devant_droite = new Vector3 (origine.x - Taille_Lasers * Mathf.Sin (angle + Mathf.PI / 4), origine.y, origine.z - Taille_Lasers * Mathf.Cos (angle + Mathf.PI / 4));

		CastRay ();

        distance_devant_gauche = hit_devant_gauche.distance;
        distance_devant = hit_devant.distance;
        distance_devant_droite = hit_devant_droite.distance;


	}

	void CastRay() {
		//left linecast
		Physics.Linecast (origine, Devant_gauche, out hit_devant_gauche);
		Debug.DrawLine (origine, Devant_gauche, Color.blue);
		//front
		Physics.Linecast (origine, Devant, out hit_devant);
		Debug.DrawLine (origine, Devant, Color.blue);
		//frontright
		Physics.Linecast (origine, Devant_droite, out hit_devant_droite);
		Debug.DrawLine (origine, Devant_droite, Color.blue);

	}
}
