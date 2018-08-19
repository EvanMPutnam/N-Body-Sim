
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class StarObj
{
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;

    public float mass = 1000f;

    public Point velocity = new Point(0, 0, 0);
    public Point acceleration = new Point(0, 0, 0);

    public StarObj(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

}


public class Point
{
    public float x;
    public float y;
    public float z;
    public Point(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

public class GenerateStars : MonoBehaviour
{




    public bool isSimulating;

    /// <summary>
    /// Stores the name for the filepath
    /// Defined in editor
    /// </summary>
    public string filePath;



    /// <summary>
    /// Whether or not the stars are color accurate
    /// </summary>
    public static Boolean COLOR_ACCURATE = false;







    /// <summary>
    /// Ammount of stars to parse from csv.  Defined in editor.
    /// Max is 119615
    /// </summary>
    public int starAmmount;

    /// <summary>
    /// Scalar to multiply the x, y, z locations by to spread them out
    /// </summary>
    private float scaler = 0.2f;




    /// <summary>
    /// Particle system to create "stars" at given locations.
    /// Defined in editor.
    /// </summary>
    public ParticleSystem partSystem;


    /// <summary>
    /// Private variable for counting stars processed
    /// </summary>
    private int starSize = 0;


    public bool moduloActivated = false;
    public int moduloAmmount = 3;


    private List<StarObj> stars = new List<StarObj>();


    /// <summary>
    /// Start function reads in the csv files and then subsequently creates the stars.
    /// </summary>
    void Start()
    {
        readCsv();
        createStars();
    }



    /// <summary>
    /// Reads the csv file and fills the x, y, z arrays with values
    /// </summary>
    private void readCsv()
    {

        DateTime d = DateTime.Now;
        using (var reader = new StreamReader("Assets/Resources/stars.csv"))
        {

            int count = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');


                if (count != 0 && count < starAmmount && ((moduloActivated && count%moduloAmmount==1) || !moduloActivated))
                {

                    //Note that y and z coordinates are flipped because unity has a different coordinate system.  Y is up/down
                    StarObj star = new StarObj(float.Parse(values[17]) * scaler, float.Parse(values[19]) * scaler, float.Parse(values[18]) * scaler);
                    //StarObj star = new StarObj(float.Parse(values[17]) * scaler, 0, float.Parse(values[18]) * scaler);
                    star.velocity.x = float.Parse(values[20]) * scaler;
                    star.velocity.y = float.Parse(values[22]) * scaler;
                    star.velocity.z = float.Parse(values[21]) * scaler;
                    star.mass = (float)Math.Pow(float.Parse((values[14])), 1/3.5);
                    if(float.IsNaN(star.mass)){
                        star.mass = 0.50f;
                    }
                    stars.Add(star);

                    partSystem.Emit(1);
                    starSize += 1;

                }
                else
                {
                    //print (values [16]);
                }

                if (count == starAmmount)
                {
                    break;
                }

                count += 1;

            }

        }
        //print (DateTime.Now.Second - d.Second );
    }



    /// <summary>
    /// Creates the stars
    /// </summary>
    private void createStars()
    {
        ParticleSystem.Particle[] arrParts;
        arrParts = new ParticleSystem.Particle[starSize];
        partSystem.GetParticles(arrParts);

        int count = 0;
        foreach (StarObj star in stars)
        {
            ParticleSystem.Particle par = arrParts[count];
            par.position = new Vector3(
                star.x, star.y, star.z
            );

            //par.velocity = Vector3.zero;
            arrParts[count] = par;
            count += 1;
            //print (count);
        }
        //print (count);
        //theParticleSystem.SetParticles (arrParticles, arrParticles.Length);
        //print(starAmmount +" "+starSize);
        partSystem.SetParticles(arrParts, starSize);
    }









    private float G_CONST = 6.67408e-11f;


    //This updates the location!
    private void calcLocation(int timeStep = 1){
        ParticleSystem.Particle[] arrParts;
        arrParts = new ParticleSystem.Particle[starSize];
        partSystem.GetParticles(arrParts);

        int count = 0;
        foreach (StarObj star in stars)
        {
            ParticleSystem.Particle par = arrParts[count];

            star.x += star.velocity.x * timeStep;
            star.y += star.velocity.y * timeStep;
            star.z += star.velocity.z * timeStep;

            par.position = new Vector3(
                star.x, star.y, star.z
            );

            //par.velocity = Vector3.zero;
            arrParts[count] = par;
            count += 1;
            //print (count);
        }
        partSystem.SetParticles(arrParts, starSize);

    }










    private Point calculateAccel(int indexOfStar){
        Point acceleration = new Point(0, 0, 0);
        StarObj starTarget = stars[indexOfStar];
        int count = 0;
        foreach(StarObj star in stars){

            if(indexOfStar != count){

                float r = (float)(Math.Pow((starTarget.x - star.x), 2)+
                                     Math.Pow((starTarget.y - star.y), 2) +
                                  Math.Pow((starTarget.z - star.z), 2));
                r = (float)Math.Sqrt(r);

                float temp = G_CONST * star.mass / (float)Math.Pow(r, 3);
                acceleration.x += temp * (star.x - starTarget.x);
                acceleration.y += temp * (star.y - starTarget.y);
                acceleration.z += temp * (star.z - starTarget.z);


            }
            count += 1;
        }
        return acceleration;

    }

    private void calcVelocity(int timeStep = 1)
    {
        int count = 0;
        foreach (StarObj star in stars)
        {
            Point acceleration = calculateAccel(count);
            star.velocity.x += acceleration.x * timeStep;
            star.velocity.y += acceleration.y * timeStep;
            star.velocity.z += acceleration.z * timeStep;
            count += 1;
        }
    }

    private void gravityStep(int timeStep = 1){
        calcVelocity(timeStep);
        calcLocation(timeStep);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
        */
        if (isSimulating)
        {
            gravityStep(10000);

        }



    }

}
