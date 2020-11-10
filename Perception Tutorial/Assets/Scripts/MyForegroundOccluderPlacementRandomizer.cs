﻿using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Perception.Randomization.Parameters;
using UnityEngine.Experimental.Perception.Randomization.Randomizers;
using UnityEngine;
using UnityEngine.Experimental.Perception.Randomization.Randomizers.SampleRandomizers;

/// <summary>
/// Creates a 2D layer of of evenly spaced GameObjects from a given list of prefabs
/// </summary>
[Serializable]
[AddRandomizerMenu("Perception/MyForeground Occluder Placement Randomizer")]
public class MyForegroundOccluderPlacementRandomizer : Randomizer
{
    List<GameObject> m_SpawnedObjects = new List<GameObject>();

    /// <summary>
    /// The Z offset component applied to the generated layer of GameObjects
    /// </summary>
    public float depth;

    /// <summary>
    /// The minimum distance between all placed objects
    /// </summary>
    public FloatParameter separationDistance;

    /// <summary>
    /// The size of the 2D area designated for object placement
    /// </summary>
    public Vector2 placementArea;

    /// <summary>
    /// The list of prefabs sample and randomly place
    /// </summary>
    public GameObjectParameter prefabs;

    /// <summary>
    /// Generates a foreground layer of objects at the start of each scenario iteration
    /// </summary>
    protected override void OnIterationStart()
    {
        if (m_SpawnedObjects == null)
            m_SpawnedObjects = new List<GameObject>();

        var seed = scenario.GenerateRandomSeed();
        var placementSamples = PoissonDiskSampling.GenerateSamples(
            placementArea.x, placementArea.y, separationDistance.Sample(), seed);
        var offset = new Vector3(placementArea.x, placementArea.y, 0f) * -0.5f;
        var parent = scenario.transform;
        foreach (var sample in placementSamples)
        {
            var instance = UnityEngine.Object.Instantiate(prefabs.Sample(), parent);
            instance.transform.position = new Vector3(sample.x, sample.y, depth) + offset;
            m_SpawnedObjects.Add(instance);
        }
        placementSamples.Dispose();
    }

    /// <summary>
    /// Deletes generated foreground objects after each scenario iteration is complete
    /// </summary>
    protected override void OnIterationEnd()
    {
        foreach (var spawnedObject in m_SpawnedObjects)
            UnityEngine.Object.Destroy(spawnedObject);
        m_SpawnedObjects.Clear();
    }
}

