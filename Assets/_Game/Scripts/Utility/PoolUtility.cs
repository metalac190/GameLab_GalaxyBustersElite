using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MonoBehavior contains Instantiate fucntion
//cannot be static AND monobehavior?
public class PoolUtility : MonoBehaviour
{
    private static Transform projectileGroup;
    public static Transform ProjectileGroup
    {
        get
        {
            if (projectileGroup == null)
                projectileGroup = new GameObject().GetComponent<Transform>();

            return projectileGroup;
        }
            
    }

    /// <summary> Instantiates Projectiles from Pool
    /// <para>  Takes most recent inactive reference in hierarchy to set active and enable. If no availalbe inactive reference, instantiates a new reference and enables. </para>
    /// <para>   Abstracted to work with Any projectile type. </para>
    /// </summary>
    /// <param name="pool"> Pool of references in hierarchy </param>
    /// <param name="spawnPoint"> Transform of position and direction to fire projectile from </param>
    /// <param name="projectileReference"> Projectile Prefab to instantiate, if needed </param>
    public static GameObject InstantiateFromPool(List<GameObject> pool, Transform spawnPoint, GameObject projectileReference)
    {
        //Static Function to call from any script

        //First checks for any available, inactive references that already exist
        foreach (GameObject projectile in pool)
        {
            if (projectile != null && projectile.activeInHierarchy == false)
            {
                //Enables and positions projectile, to reuse
                projectile.transform.parent = ProjectileGroup;
                projectile.transform.position = spawnPoint.position;
                projectile.transform.rotation = spawnPoint.rotation;
                projectile.SetActive(true);
                return projectile;
            }
        }

        //Worst Case Scenario, instantiates for current and future use
        GameObject newProjectile = Instantiate(projectileReference, spawnPoint.position, spawnPoint.rotation, null);
        pool.Add(newProjectile);
        return newProjectile;
    }

    /// <summary> Instantiates Projectiles from Pool
    /// <para> Overload to use custom Position/Rotation and not Transform </para>
    /// </summary>
    /// <param name="pool"> Pool of references in hierarchy </param>
    /// <param name="position"> Custom Position to spawn/move projectile </param>
    /// <param name="rotation"> Custom Rotation to aim projectile </param>
    /// <param name="projectileReference"> Projectile Prefab to instantiate, if needed </param>
    /// <returns></returns>
    public static GameObject InstantiateFromPool(List<GameObject> pool, Vector3 position, Quaternion rotation, GameObject projectileReference)
    {
        //Static Function to call from any script

        //First checks for any available, inactive references that already exist
        foreach (GameObject projectile in pool)
        {
            if (projectile != null && projectile.activeInHierarchy == false)
            {
                //Enables and positions projectile, to reuse
                projectile.transform.position = position;
                projectile.transform.rotation = rotation;
                projectile.SetActive(true);
                return projectile;
            }
        }

        //Worst Case Scenario, instantiates for current and future use
        GameObject newProjectile = Instantiate(projectileReference, position, rotation, null);
        pool.Add(newProjectile);
        return newProjectile;
    }

	/// <summary> Instantiates Projectiles from Pool
	/// <para> Overload to only handle creation and setting active/inactive </para>
	/// </summary>
	/// <param name="pool"> Pool of references in hierarchy </param>
	/// <param name="projectileReference"> Projectile Prefab to instantiate </param>
	/// <param name="parentReference"> Parent to instantiate prefab under, if needed </param>
	/// <returns></returns>
	public static GameObject InstantiateFromPool(List<GameObject> pool, GameObject projectileReference, Transform parentReference = null)
	{
		//Static Function to call from any script

		//First checks for any available, inactive references that already exist
		foreach (GameObject projectile in pool)
		{
			if (projectile != null && projectile.activeInHierarchy == false)
			{
				//Enables projectile, to reuse
				projectile.SetActive(true);
				return projectile;
			}
		}

		//Worst Case Scenario, instantiates for current and future use
		GameObject newProjectile = Instantiate(projectileReference, parentReference);
		pool.Add(newProjectile);
		return newProjectile;
	}
}