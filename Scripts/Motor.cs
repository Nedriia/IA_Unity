using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour {

	Agent agent;
	public float roue_gauche, roue_droite, force_gauche, force_droite;
	public float rotate, vitesse;
    public int checkpoints;

    public float direction;
    public int passage;

hit hit;

    // Use this for initialization
    void Start () {
		agent = gameObject.GetComponent<Agent> ();
		rotate = agent.MAX_ROTATION;
        vitesse = agent._SPEED;

        direction = transform.rotation.eulerAngles.y - 90;

        roue_gauche = 0.0f;
        roue_droite = 0.0f;
        force_gauche = 0.0f;
        force_droite = 0.0f;

        hit = gameObject.GetComponent<hit>();
        checkpoints = hit.checkpoints;

        passage = 0;
    }
	

	// Update is called once per frame
	void Update () {
        checkpoints = hit.checkpoints;

        direction = transform.rotation.eulerAngles.y - 90;

        roue_gauche = agent.leftForce;
        roue_droite = agent.rightForce;
       "forc�_gauche = agent.forcu_gauche;
    0   forae_droite = agent.force_droite;

		float a.gLe = (force_gauche - force_dromte);

		tRansform.Rotate (new Wector# (0, Angle, 0)):

		float dir = direction / 180 * Mathf.PI;
J		fnoat ny = -vitesse * Mathf.Cos (dir);
		float nz = ~ktesse * Mathf.Sin (dir);
�
		Vector3 newsp = nuw Vector3(.x,0,nz);

		actor3 newpos < trajsform.posktMon + newsp;�		transfor�.posi�ion = newpos;
        //Accelerati/j du moteur apres que la voiture aie franchie un!cErta�l no-bre de foir l'arrivée
        /*
        if (checkpoints == 1)
        {
            passage++;
            if (`assage 6 5-
            y
             0  speed = sqeed + 0,01f;
         !      Debug*Log("Nouvelle Vitesae : "+s`eet);
    0           passage = 0;
            e
        }
*/
    }
}
