using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] int AsteroidstoSpawn = 20;
    [SerializeField] float Xborder = 10;
    [SerializeField] float Yborder = 10;
    [SerializeField] Synchronizer synchronizer;
    [SerializeField] GameObject AsteroidTemplate;

    private GameObject Asteroid;
    private AsteroidLogic Logic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [ContextMenu("Generate Asteroids")]
    void GenerateAsteroids()
    {
        float n = 0;
        float x = 0;
        float y = 0;
        bool Run = true;
        for (int i = 0;  i < AsteroidstoSpawn; i++)
        {
            Asteroid = GameObject.Instantiate(AsteroidTemplate,this.transform);
            Logic = Asteroid.GetComponent<AsteroidLogic>();
            Logic.SyncRef = synchronizer;

            n = Random.Range(-1f,1f);
            x = Random.Range(-1f, 1f);
            y = Random.Range(-1f, 1f);

            if (n > 0)
            {
                Logic.A.transform.position = new Vector3(Mathf.Sign(x)*Xborder, y*Yborder,0);
            }
            else
            {
                Logic.A.transform.position = new Vector3(x * Xborder, Mathf.Sign(y) * Yborder, 0);
            }
            Run = true;
            while (Run) 
            {
                n = Random.Range(-1f, 1f);
                x = Random.Range(-1f, 1f);
                y = Random.Range(-1f, 1f);

                if (n > 0)
                {
                    Logic.B.transform.position = new Vector3(Mathf.Sign(x) * Xborder, y * Yborder, 0);
                }
                else
                {
                    Logic.B.transform.position = new Vector3(x * Xborder, Mathf.Sign(y) * Yborder, 0);
                }

                if (Mathf.Abs(Logic.A.position.x - Logic.B.position.x) > Xborder/2 && Mathf.Abs(Logic.A.position.y - Logic.B.position.y) > Yborder / 2) Run = false;
            }
            

            Logic.StartingRatio = Random.Range(0f,1f);
            Logic.StartingRotation = Random.Range(0f,360f);
        }
    }

}
