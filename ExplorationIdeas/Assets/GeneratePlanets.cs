using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;


public class PlanetObj{
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;

    public float mass = 1000f;

    public Point velocity = new Point(0, 0, 0);
    public Point acceleration = new Point(0, 0, 0);

    public GameObject planetSphere;
    public Color color = Color.white;

    public float diameter = 0.0f;

    public PlanetObj(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

}

public class GeneratePlanets : MonoBehaviour {


    List<PlanetObj> planets = new List<PlanetObj>();




    public float scaleFactor = 0.01f;
    public int scaleFactor1 = 1;
    private float G_CONST = 6.67408e-11f;



    /*
     * 
        sun = {"location":point(0,0,0), "mass":2e30, "velocity":point(0,0,0)}
        mercury = {"location":point(0,5.7e10,0), "mass":3.285e23, "velocity":point(47000,0,0)}
        venus = {"location":point(0,1.1e11,0), "mass":4.8e24, "velocity":point(35000,0,0)}
        earth = {"location":point(0,1.5e11,0), "mass":6e24, "velocity":point(30000,0,0)}
        mars = {"location":point(0,2.2e11,0), "mass":2.4e24, "velocity":point(24000,0,0)}
        jupiter = {"location":point(0,7.7e11,0), "mass":1e28, "velocity":point(13000,0,0)}
        saturn = {"location":point(0,1.4e12,0), "mass":5.7e26, "velocity":point(9000,0,0)}
        uranus = {"location":point(0,2.8e12,0), "mass":8.7e25, "velocity":point(6835,0,0)}
        neptune = {"location":point(0,4.5e12,0), "mass":1e26, "velocity":point(5477,0,0)}
        pluto = {"location":point(0,3.7e12,0), "mass":1.3e22, "velocity":point(4748,0,0)}

     */
    // Use this for initialization
    void Start(){
        
        G_CONST = G_CONST * scaleFactor;
        int[] planetDiams = new int[] {1391016, 4879, 12104, 12756, 6792, 142984, 120536, 51118, 49528, 2370};
        PlanetObj sun = new PlanetObj(0, 0, 0);
        sun.velocity = new Point(0, 0, 0);
        sun.mass = 2e30f * scaleFactor1;
        sun.color = Color.yellow;
        planets.Add(sun);

        PlanetObj mercury = new PlanetObj(0, 5.7e10f*scaleFactor, 0);
        mercury.velocity = new Point(47000*scaleFactor1, 0, 0);
        mercury.mass = 3.285e23f*scaleFactor1;
        mercury.color = Color.grey;
        planets.Add(mercury);

        PlanetObj venus = new PlanetObj(0, 1.1e11f*scaleFactor, 0);
        venus.velocity = new Point(35000*scaleFactor1, 0, 0);
        venus.mass = 4.8e24f*scaleFactor1;
        venus.color = Color.red;
        planets.Add(venus);

        PlanetObj earth = new PlanetObj(0, 1.5e11f*scaleFactor, 0);
        earth.velocity = new Point(30000*scaleFactor1, 0, 0);
        earth.mass = 6e24f * scaleFactor1;
        earth.color = Color.green;
        planets.Add(earth);

        PlanetObj mars = new PlanetObj(0, 2.2e11f * scaleFactor, 0);
        mars.velocity = new Point(24000 * scaleFactor1, 0, 0);
        Debug.Log(mars.velocity.x);
        mars.mass = 2.4e24f * scaleFactor1;
        mars.color = Color.red;
        planets.Add(mars);

        PlanetObj jupiter = new PlanetObj(0, 7.7e11f * scaleFactor, 0);
        jupiter.velocity = new Point(13000 * scaleFactor1, 0, 0);
        jupiter.mass = 1e28f * scaleFactor1;
        jupiter.color = Color.yellow;
        planets.Add(jupiter);

        PlanetObj saturn = new PlanetObj(0, 1.4e12f * scaleFactor, 0);
        saturn.velocity = new Point(9000 * scaleFactor1, 0, 0);
        saturn.mass = 5.7e26f * scaleFactor1;
        saturn.color = Color.red;
        planets.Add(saturn);

        PlanetObj uranus = new PlanetObj(0, 2.8e12f * scaleFactor, 0);
        uranus.velocity = new Point(6835 * scaleFactor1, 0, 0);
        uranus.mass = 8.7e25f * scaleFactor1;
        uranus.color = Color.grey;
        planets.Add(uranus);

        PlanetObj neptune = new PlanetObj(0, 4.5e12f * scaleFactor, 0);
        neptune.velocity = new Point(5477 * scaleFactor1, 0, 0);
        neptune.mass = 1e26f * scaleFactor1;
        neptune.color = Color.blue;
        planets.Add(neptune);

        PlanetObj pluto = new PlanetObj(0, 3.7e12f * scaleFactor, 0);
        pluto.velocity = new Point(4748 * scaleFactor1, 0, 0);
        pluto.mass = 1.3e22f * scaleFactor1;
        pluto.color = Color.red;
        planets.Add(pluto);

        int count = 0;
        foreach(int diam in planetDiams){
            float f;
            if (count == 0)
            {
                f = diam / 2000;
            }else{
                f = diam / 100;
            }
            PlanetObj planet = planets[count];
            planet.diameter = f;
            planet.planetSphere = GameObject.CreatePrimitive(
                PrimitiveType.Sphere);
            planet.planetSphere.transform.position = new Vector3(planet.x, 
                                                                 planet.y, 
                                                                 planet.z);
            planet.planetSphere.transform.localScale = new Vector3(f, f, f);
            planet.planetSphere.GetComponent<Renderer>().material.color = planet.color;
            count += 1;
        }


    }



    //This updates the location!
    private void calcLocation(float timeStep = 1)
    {

        foreach (PlanetObj star in planets)
        {


            star.x += star.velocity.x * timeStep;
            star.y += star.velocity.y * timeStep;
            star.z += star.velocity.z * timeStep;

            star.planetSphere.transform.position = new Vector3(
                star.x, star.y, star.z
            );

            //par.velocity = Vector3.zero;
            //print (count);
        }

    }

    private Point calculateAccel(int indexOfStar)
    {
        Point acceleration = new Point(0, 0, 0);
        PlanetObj starTarget = planets[indexOfStar];
        int count = 0;
        foreach (PlanetObj star in planets)
        {

            if (indexOfStar != count)
            {

                float r = (float)(Math.Pow((starTarget.x - star.x), 2) +
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

    private void calcVelocity(float timeStep = 1)
    {
        int count = 0;
        foreach (PlanetObj star in planets)
        {
            Point acceleration = calculateAccel(count);

            star.velocity.x += acceleration.x * timeStep;
            star.velocity.y += acceleration.y * timeStep;
            star.velocity.z += acceleration.z * timeStep;
            count += 1;
        }
    }

    private void gravityStep(float timeStep = 1)
    {
        calcVelocity(timeStep);
        calcLocation(timeStep);
    }


	// Update is called once per frame
	void Update () {
        gravityStep(.0001f);
	}
}
