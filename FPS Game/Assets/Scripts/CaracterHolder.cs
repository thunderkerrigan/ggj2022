using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CaracterHolder : MonoBehaviour
{
    // Start is called before the first frame update

   public void updateMaterial(int index)
    {
        var material = Resources.Load<Material>("Baby Material/Cartoon__Baby_0" + index);
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>().ToList())
        {
            skinnedMeshRenderer.material = material;
        }
    }
    
}
