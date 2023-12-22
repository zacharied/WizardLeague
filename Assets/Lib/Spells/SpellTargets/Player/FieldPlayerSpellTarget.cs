using UnityEngine;

namespace Lib.SpellTargets
{
    public class FieldPlayerSpellTarget : MonoBehaviour, ISpellTarget
    {
        public GameObject TargetPreviewObject;

        private GameObject instantiatedObject;
    
        public void DrawTarget()
        {
            Debug.Log("Drawing target");
            var canvas = FindObjectOfType<Canvas>();
            instantiatedObject = Instantiate(TargetPreviewObject, canvas.transform);
        }

        public void ClearTarget()
        {
            Destroy(instantiatedObject); 
        }
    }
}