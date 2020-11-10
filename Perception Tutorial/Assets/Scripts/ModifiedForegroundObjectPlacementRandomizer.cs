using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Perception.Randomization.Parameters;
using UnityEngine.Experimental.Perception.Randomization.Randomizers;
using UnityEngine.Experimental.Perception.Randomization.Randomizers.SampleRandomizers;
using Object = UnityEngine.Object;


/// <summary>
/// Creates a 2D layer of of evenly spaced GameObjects from a given list of prefabs
/// </summary>
[Serializable]
[AddRandomizerMenu("Perception/Dual Layer Foreground Object Placement Randomizer")]
public class ModifiedForegroundObjectPlacementRandomizer : Randomizer
{
    public int MaxObjectCount;
    
    List<GameObject> m_LayerOneSpawnedObjects = new List<GameObject>();
    public float layerOneDepth;
    public FloatParameter layerOneSeparationDistance;
    public FloatParameter layerOneDepthDisplacement;
    public Vector2 layerOnePlacementArea;
    public GameObjectParameter layerOnePrefabs;
    
    
    List<GameObject> m_LayerTwoSpawnedObjects = new List<GameObject>();
    public float layerTwoDepth;
    public FloatParameter layerTwoSeparationDistance;
    public FloatParameter layerTwoDepthDisplacement;
    public Vector2 layerTwoPlacementArea;
    public GameObjectParameter layerTwoPrefabs;

    /// <summary>
    /// Generates a foreground layer of objects at the start of each scenario iteration
    /// </summary>
    protected override void OnIterationStart()
    {
        PlaceLayerOneObjects();
        PlaceLayerTwoObjects();
        TrimObjects();
    }


    void PlaceLayerOneObjects()
    {
        if (m_LayerOneSpawnedObjects == null)
            m_LayerOneSpawnedObjects = new List<GameObject>();

        var seed = scenario.GenerateRandomSeed();
        var placementSamples = PoissonDiskSampling.GenerateSamples(
            layerOnePlacementArea.x, layerOnePlacementArea.y, layerOneSeparationDistance.Sample(), seed);
        var offset = new Vector3(layerOnePlacementArea.x, layerOnePlacementArea.y, 0) * -0.5f;
        var parent = scenario.transform;
        foreach (var sample in placementSamples)
        {
            var instance = Object.Instantiate(layerOnePrefabs.Sample(), parent);
            instance.transform.position = new Vector3(sample.x, sample.y, layerOneDepth + layerOneDepthDisplacement.Sample()) + offset;
            m_LayerOneSpawnedObjects.Add(instance);
        }
        placementSamples.Dispose();
    }


    void PlaceLayerTwoObjects()
    {
        if (m_LayerTwoSpawnedObjects == null)
            m_LayerTwoSpawnedObjects = new List<GameObject>();

        var seed = scenario.GenerateRandomSeed();
        var placementSamples = PoissonDiskSampling.GenerateSamples(
            layerTwoPlacementArea.x, layerTwoPlacementArea.y, layerTwoSeparationDistance.Sample(), seed);
        var offset = new Vector3(layerTwoPlacementArea.x, layerTwoPlacementArea.y, 0) * -0.5f;
        var parent = scenario.transform;
        foreach (var sample in placementSamples)
        {
            var instance = Object.Instantiate(layerTwoPrefabs.Sample(), parent);
            instance.transform.position = new Vector3(sample.x, sample.y, layerTwoDepth + layerTwoDepthDisplacement.Sample()) + offset;
            m_LayerTwoSpawnedObjects.Add(instance);
        }
        placementSamples.Dispose();
    }

    void TrimObjects()
    {
        var r = new System.Random();
        while (m_LayerOneSpawnedObjects.Count + m_LayerTwoSpawnedObjects.Count > 99)
        {
            var obj = m_LayerOneSpawnedObjects.ElementAt(r.Next(0, m_LayerOneSpawnedObjects.Count()));
            m_LayerOneSpawnedObjects.Remove(obj);
            Object.Destroy(obj);

            if (m_LayerOneSpawnedObjects.Count + m_LayerTwoSpawnedObjects.Count <= 99) continue;
            
            obj = m_LayerTwoSpawnedObjects.ElementAt(r.Next(0, m_LayerTwoSpawnedObjects.Count()));
            m_LayerTwoSpawnedObjects.Remove(obj);
            Object.Destroy(obj);
        }
    }
    
    /// <summary>
    /// Deletes generated foreground objects after each scenario iteration is complete
    /// </summary>
    protected override void OnIterationEnd()
    {
        foreach (var spawnedObject in m_LayerOneSpawnedObjects)
            Object.Destroy(spawnedObject);
        m_LayerOneSpawnedObjects.Clear();
        
        
        foreach (var spawnedObject in m_LayerTwoSpawnedObjects)
            Object.Destroy(spawnedObject);
        m_LayerTwoSpawnedObjects.Clear();
    }
}

