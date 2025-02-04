
namespace Assets.Scripts.Models
{
    [System.Serializable]
    public class Root
    {
        public Domain[] domains;

        public override string ToString()
        {
            return $"domains: {domains}";
        }
    }

    [System.Serializable]
    public class Domain
    {
        public long id;
        public string nombre;
        public Chapter[] capitulos;

        public override string ToString()
        {
            return $"id: {id} - nombre: {nombre}";
        }
    }

    [System.Serializable]
    public class Chapter
    {
        public long id;
        public string nombre;
        public Question[] preguntas;

        public override string ToString()
        {
            return $"id: {id} - nombre: {nombre}";
        }
    }

    [System.Serializable]
    public class Question
    {
        public long id;
        public string pregunta;
        public Alternativa[] alternativas;
        public long respuesta_correcta_id;

        public override string ToString()
        {
            return $"id: {id} - pregunta: {pregunta}";
        }
    }

    [System.Serializable]
    public class Alternativa
    {
        public long id;
        public string texto;
    }
}