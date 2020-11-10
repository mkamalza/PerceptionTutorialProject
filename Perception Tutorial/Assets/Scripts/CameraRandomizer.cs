using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Perception.Randomization.Randomizers;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using FloatParameter = UnityEngine.Experimental.Perception.Randomization.Parameters.FloatParameter;

[Serializable]

[AddRandomizerMenu("Perception/My Camera Randomizer")]
public class CameraRandomizer : Randomizer
{
    public FloatParameter nearFieldParameter;
    public FloatParameter contrastParameter;
    public FloatParameter saturationParameter;
    
    protected override void OnIterationStart()
    {
        var taggedObjects = tagManager.Query<CameraRandomizerTag>();
        foreach (var taggedObject in taggedObjects)
        {
            var volume = taggedObject.GetComponent<Volume>();
            if (volume && volume.profile)
            {
                var dof = (DepthOfField) volume.profile.components.Find(comp => comp is DepthOfField);
                if (dof)
                {
                    float val = nearFieldParameter.Sample();
                    dof.gaussianStart.min = 0;
                    dof.gaussianStart.value = val;
                    
                    dof.gaussianEnd.min = 0;
                    dof.gaussianEnd.value = val;
                }

                var colorAdjust = (ColorAdjustments) volume.profile.components.Find(comp => comp is ColorAdjustments);
                if (colorAdjust)
                {
                    float val = contrastParameter.Sample();
                    colorAdjust.contrast.min = val;
                    colorAdjust.contrast.max = val;

                    val = saturationParameter.Sample();
                    colorAdjust.saturation.min = val;
                    colorAdjust.saturation.max = val;
                }
            }
        }
    }
}