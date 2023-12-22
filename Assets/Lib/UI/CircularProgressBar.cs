using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Lib.UI
{
    [UnityEngine.Scripting.Preserve]
    public class CircularProgressBar : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<CircularProgressBar, UxmlTraits>
        { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlFloatAttributeDescription progress = new UxmlFloatAttributeDescription()
                { name = "progress", defaultValue = 20f };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = (CircularProgressBar)ve;

                ate.progress = progress.GetValueFromBag(bag, cc);
            }
        }

        private float _progress;

        public float progress
        {
            get => _progress;
            set
            {
                _progress = Mathf.Clamp(value, 0, 100); 
                MarkDirtyRepaint(); 
            }
        }

        public CircularProgressBar()
        {
            generateVisualContent += OnGenerateVisualContent;
        }
        
        void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            var paint = mgc.painter2D;
            paint.BeginPath();
            paint.Arc(contentRect.center, contentRect.size.x / 2, 0, -(360 * progress / 100), ArcDirection.CounterClockwise);
            paint.lineWidth = 4f;
            paint.Stroke();
        }
    }
}