using UnityEngine;
using UnityEngine.Experimental.Perception.Randomization.Randomizers;

[AddComponentMenu("Perception/RandomizerTags/MyLightSwitcherTag")]
public class MyLightSwitcherTag : RandomizerTag
{
    public void Act(float rawInput)
    {
        var light = gameObject.GetComponent<Light>();
        if (light)
        {
            light.enabled = rawInput > 0.5f;
        }
    }
}
