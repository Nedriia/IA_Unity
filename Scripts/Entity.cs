using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Entity : MonoBehaviour {

	Agent testAgent;
    
    //utiliser pour ditance = fitness
    Agent fitness;
    public float rotate, vitesse;

    //pourcentage circuit
    public float tot;

    //private List<Agent> agents;
	public float Fitness_actuelle;
	public float Meilleur_fitness;
    //private float currentTimer;
    //private int checkPointsHit;

    //Temps au tour
    public float chrono;
    public float meilleurchrono;
    public int compteur;
    public float meilleurgen;
    public int Arrivee;
    public float recompense;

    List<string> perfchrono = new List<string>();
    List<string> perfitness = new List<string>();
    

    //Reseau
    public NNet neuralNet;

	public GA genAlg;
	public int checkpoints;
	public GameObject[] CPs;
	public Material normal;

	private Vector3 defaultpos;
	private Quaternion defaultrot;

	hit hit;

	public void OnGUI(){
		int x = 300;
		int y = 300;
		GUI.Label (new Rect (x, y, 200, 20), "Fitness actuelle : " + Fitness_actuelle);
		GUI.Label (new Rect (x, y+20, 300, 20), "Meilleur Fitness : " + Meilleur_fitness);
		GUI.Label (new Rect (x, y+80, 200, 20), "Individu : " + genAlg.currentGenome + " sur " + genAlg.totalPopulation);
		GUI.Label (new Rect (x, y +100, 200, 20), "Génération : " + genAlg.generation);

    }


	// Use this for initialization
	void Start () {
        //pourcentage du circuit
        //circuit 1, moyenne des valeurs parcourus par 30 générations
        tot = 32.0f;
        //circuit 2

        //circuit 3

        fitness = gameObject.GetComponent<Agent>();
        rotate = fitness.MAX_ROTATION;
        vitesse = fitness._SPEED;

        compteur = 1;
        meilleurgen = 0;
        Arrivee = 0;
        meilleurchrono = 100.0f;
        chrono = 0.0f;

		genAlg = new GA ();
		int totalWeights = 3 * 17 + 4 * 2 + 4 + 2;
        genAlg.GenerateNewPopulation (15, totalWeights);

        Fitness_actuelle = 0.0f;
        Meilleur_fitness = 0.0f;

		neuralNet = new NNet ();
		neuralNet.CreateNet (1, 3, 4, 2);
		Genome genome = genAlg.GetNextGenome ();
		neuralNet.FromGenome (genome, 3, 4, 2);

		testAgent = gameObject.GetComponent<Agent>();
		testAgent.Attach (neuralNet);

        //Utiliser pour gérer le temps au tour, passage à l'arrivée
		hit = gameObject.GetComponent<hit> ();
		checkpoints = hit.checkpoints;
		defaultpos = transform.position;
		defaultrot = transform.rotation;
        recompense = 0.0f;
    }

	// Update is called once per frame
	void Update () {

        checkpoints = hit.checkpoints;
        if (testAgent.hasFailed)
        {
            if (genAlg.GetCurrentGenomeIndex() == 15 - 1)
            {
                EvolveGenomes();
                return;
            }
            NextTestSubject();
        }
        //test de changement de la fitness
        //vitesse * durée = distance
        Fitness_actuelle = vitesse *(chrono*60);

        //test de calcul de distance parcourue
        //Fitness_actuelle = testAgent.dist;
        if (Fitness_actuelle > Meilleur_fitness)
        {
            Meilleur_fitness = Fitness_actuelle;
        }

        if (checkpoints == 0)
        {
            chrono = hit.chrono;
        }
        else
        {
            Fitness_actuelle = Fitness_actuelle + 5;
      
                if (Fitness_actuelle > Meilleur_fitness)
                {
                    Meilleur_fitness = Fitness_actuelle;
                }
                
            //Temps au tour, avec mise en place récompense, à déterminer pour pas creer de fossé
            if (chrono < meilleurchrono)
            {
                meilleurchrono = chrono;
                //currentAgentFitness  = currentAgentFitness + recompense;
                //recompense = recompense + 5;
                if (Fitness_actuelle > Meilleur_fitness)
                {
                    Meilleur_fitness = Fitness_actuelle;
                }
                string test = System.Convert.ToString(meilleurchrono);
                perfchrono.Add(test);
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"C:\Users\arthu\Desktop\test\meilleurtemps.txt", true))
                {
                    file.WriteLine(test);
                }
            }
            
            NextTestSubject();
        }
    }
    
    //Récupération des données de Fitness par Individus
    public void NextTestSubject(){
        //Pourcentage du circuit accomplie
        float pourcentage =(Fitness_actuelle/tot)*100;
        if (pourcentage > 100|| checkpoints != 0)
        {
            pourcentage = 100;
            Debug.Log("Circuit terminé à " + pourcentage + "% !");
        }
        else
        {
            Debug.Log("Echec à "+pourcentage + " %");
        }
        

        string perf1 = System.Convert.ToString(Fitness_actuelle);
        perfitness.Add(perf1);
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\arthu\Desktop\test\fitness.txt", true))
            {
                file.WriteLine(perf1+",");
            }

        //Récupération des données de Fitness par génération
        compteur++;
        float test = Fitness_actuelle;
        if (test > meilleurgen){
           meilleurgen = test;
        }
        if (compteur == 15){
            string perf2 = System.Convert.ToString(meilleurgen);
            perfitness.Add(perf2);
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\arthu\Desktop\test\fitnessgen.txt", true))
            {
                file.WriteLine(perf2+",");
            }
            compteur = 1;
            meilleurgen = 0;
        }
        
        genAlg.SetGenomeFitness (Fitness_actuelle, genAlg.GetCurrentGenomeIndex ());
        Fitness_actuelle = 0.0f;
		Genome genome = genAlg.GetNextGenome ();

		neuralNet.FromGenome (genome, 3, 4, 2);

		transform.position = defaultpos;
		transform.rotation = defaultrot;

		testAgent.Attach (neuralNet);
		testAgent.ClearFailure ();


		//reset the checkpoints
		CPs = GameObject.FindGameObjectsWithTag ("Checkpoint");

		foreach (GameObject c in CPs) {
			Renderer tmp = c.gameObject.GetComponent<Renderer>();
			Checkpoint p = c.gameObject.GetComponent<Checkpoint>();
			p.passed = false;
		}
	}

	public void BreedNewPopulation(){
		genAlg.ClearPopulation ();
		int totalweights = 3 * 17 + 4 * 2 + 4 + 2;
		genAlg.GenerateNewPopulation (15, totalweights);
	}

	public void EvolveGenomes(){
		genAlg.BreedPopulation ();
		NextTestSubject ();
	}

	public int GetCurrentMemberOfPopulation(){
		return genAlg.GetCurrentGenomeIndex ();
	}

}
