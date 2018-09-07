using UnityEngine;
using System.Collections;

public class hit : MonoBehaviour {
	public int checkpoints;
	public Vector3 init_pos;
	public Quaternion init_rotation;
	public bool crash;
    public float chrono;

	Agent agent;

	// Use this for initialization

	void Start () {
        chrono = 0.0f;
		agent = gameObject.GetComponent<Agent> ();
		crash = false;
		checkpoints = 0;
		init_pos = transform.position;
		init_rotation = transform.rotation;
	}
	
	// Update is called once per frame

	void Update () {
        chrono += Time.deltaTime;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Checkpoint") {
			Renderer tmp = other.gameObject.GetComponent<Renderer> ();
			Checkpoint t = other.gameObject.GetComponent<Checkpoint>();
			bool p = t.passed;
			if(!p){
				t.SetBool(true);
				checkpoints++;
				agent.dist += 1.0f;
                chrono = 0.0f;
			}
		} else {
			crash = true;
            chrono = 0.0f;
		}
	}
}
