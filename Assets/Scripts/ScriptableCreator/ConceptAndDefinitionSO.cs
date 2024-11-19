using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableCreator
{
    [Serializable]
    public class ConceptAndDefinitionData
    {
        public int id;
        public string concept;
        public string definition;
    }
    
    [CreateAssetMenu(fileName = "ConceptAndDefinitionData", menuName = "Concepts and definitions")]
    public class ConceptAndDefinitionSO : ScriptableObject
    {
        public List<ConceptAndDefinitionData> list;
    }
}
