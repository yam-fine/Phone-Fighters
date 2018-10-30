///////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Filename: SpawnAI.cs
//  
// Author: Garth de Wet <garthofhearts@gmail.com>
// Website: http://corruptedsmilestudio.blogspot.com/
// Date Modified: 15 November 2013
//
// Copyright (c) 2013 Garth de Wet
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using CorruptedSmileStudio.Spawn;

/// <summary>
/// SpawnAI component. Provides the base methods required to interact with the spawner.
/// </summary>
/// <description>
/// This component can be used out of the box on existing GameObjects that get spawned.<br />
/// This class interacts with the Spawner and handles all interaction between the two. It also makes
/// use of InstanceManager in order to work with Pool Manager by Path-o-Logical (if it is installed).<br />
/// Simply interact with this class in your killing method and call Remove() on this component last in the method,
/// you will not need to call Destroy(gameObject) in order to destroy the GameObject as this class handles all of that.
/// </description>
public class SpawnAI : MonoBehaviour
{
	/// <summary>
	/// The Spawner owner of the spawned unit
	/// </summary>
	private Spawner mOwner;
	/// <summary>
	/// Remove this unit from the owner and Despawns the unit via InstanceManager
	/// </summary>
    public virtual void Remove()
    {
		if(mOwner != null)
		{
			mOwner.RemoveUnit();
		}
		//InstanceManager.Despawn(transform);
    }
	/// <summary>
	/// Sets the owner.
	/// </summary>
	/// <param name="owner">The Spawner that spawned this unit.</param>
	public virtual void SetOwner(Spawner owner)
    {
		mOwner = owner;
    }
}