using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Utils
{
    [AddComponentMenu("UI/Effects/Directional Outline", 82)]
    public class DirectionalOutline : Shadow
    {
        [SerializeField] private bool m_DrawTop = true;
        [SerializeField] private bool m_DrawBottom = true;
        [SerializeField] private bool m_DrawLeft = true;
        [SerializeField] private bool m_DrawRight = true;

        public bool drawTop
        {
            get => m_DrawTop;
            set
            {
                if (m_DrawTop == value) return;
                m_DrawTop = value;
                graphic?.SetVerticesDirty();
            }
        }

        public bool drawBottom
        {
            get => m_DrawBottom;
            set
            {
                if (m_DrawBottom == value) return;
                m_DrawBottom = value;
                graphic?.SetVerticesDirty();
            }
        }

        public bool drawLeft
        {
            get => m_DrawLeft;
            set
            {
                if (m_DrawLeft == value) return;
                m_DrawLeft = value;
                graphic?.SetVerticesDirty();
            }
        }

        public bool drawRight
        {
            get => m_DrawRight;
            set
            {
                if (m_DrawRight == value) return;
                m_DrawRight = value;
                graphic?.SetVerticesDirty();
            }
        }

        protected DirectionalOutline() { }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            var verts = ListPool<UIVertex>.Get();
            vh.GetUIVertexStream(verts);

            // Calcular cuántas direcciones están activas para reservar capacidad
            int activeDirections = 0;
            if (m_DrawTop) activeDirections++;
            if (m_DrawBottom) activeDirections++;
            if (m_DrawLeft) activeDirections++;
            if (m_DrawRight) activeDirections++;

            int neededCapacity = verts.Count * (activeDirections + 1);
            if (verts.Capacity < neededCapacity)
                verts.Capacity = neededCapacity;

            int start = 0;
            int end = verts.Count;

            // Cada dirección desplaza los vértices en el eje correspondiente.
            if (m_DrawTop)
            {
                ApplyShadowZeroAlloc(verts, effectColor, start, end, 0f, effectDistance.y);
                start = verts.Count;
            }

            if (m_DrawBottom)
            {
                ApplyShadowZeroAlloc(verts, effectColor, start, end, 0f, -effectDistance.y);
                start = verts.Count;
            }

            if (m_DrawLeft)
            {
                ApplyShadowZeroAlloc(verts, effectColor, start, end, -effectDistance.x, 0f);
                start = verts.Count;
            }

            if (m_DrawRight)
            {
                ApplyShadowZeroAlloc(verts, effectColor, start, end, effectDistance.x, 0f);
                start = verts.Count;
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(verts);
            ListPool<UIVertex>.Release(verts);
        }
    }
}