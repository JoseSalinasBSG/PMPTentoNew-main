//old code for TTS:
//********************************************************************************************************************

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Text.RegularExpressions;
// using UnityEngine.Networking;

// public class ByteToAudioSource : MonoBehaviour
// {
//     [SerializeField] private AudioSource _audioSource; // AudioSource para reproducir los audios.
//     [SerializeField] private string text; // Texto de entrada para convertir en audio.
//     [SerializeField] private List<string> parts; // Partes del texto dividido.

//     private const string API_URL = "https://translate.google.com/translate_tts"; // URL de la API de Google.
//     private const string LANG = "es"; // Idioma para la síntesis de texto.
//     private const int MAX_LENGTH = 70; // Longitud máxima permitida para cada segmento.

//     private string GenerateUrl(string text, string lang)
//     {
//         var encodedText = UnityWebRequest.EscapeURL(text);
//         return $"{API_URL}?ie=UTF-8&tl={lang}&client=tw-ob&q={encodedText}";
//     }

//     public void StartTTS(string text)
//     {
//         this.text = text;
//         StartCoroutine(IStartTTS());
//     }

//     public void StopTTS()
//     {
//         StopAllCoroutines();
//         _audioSource.Stop();
//     }

//     private IEnumerator IStartTTS()
//     {
//         parts = SplitText(text);
//         List<AudioClip> audioClips = new List<AudioClip>();

//         // Descargar todos los audios y almacenarlos en una lista.
//         foreach (string part in parts)
//         {
//             var url = GenerateUrl(part, LANG);
//             using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
//             {
//                 yield return www.SendWebRequest();

//                 if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
//                 {
//                     Debug.LogError("Error al descargar el audio: " + www.error);
//                 }
//                 else
//                 {
//                     AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
//                     audioClips.Add(clip);
//                 }
//             }
//         }

//         // Reproducir los audios en secuencia.
//         foreach (var clip in audioClips)
//         {
//             _audioSource.clip = clip;
//             _audioSource.Play();
//             yield return new WaitForSeconds(clip.length - 0.3f);
//         }
//     }

//     private List<string> SplitText(string text, int maxLength = MAX_LENGTH)
//     {
//         List<string> parts = new List<string>();
//         var sentences = Regex.Split(text, @"(?<=[.!?¿()]) +"); // Divide el texto en oraciones.

//         foreach (string sentence in sentences)
//         {
//             if (sentence.Length <= maxLength)
//             {
//                 parts.Add(sentence);
//             }
//             else
//             {
//                 // Si la oración es demasiado larga, la divide en fragmentos más pequeños.
//                 parts.AddRange(Regex.Split(sentence, @"(?<=[-:,.!¿?]) +"));
//             }
//         }
//         return parts;
//     }
// }

//********************************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class ByteToAudioSource : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource; // AudioSource para reproducir los audios.
    [SerializeField] private string text; // Texto de entrada para convertir en audio.
    [SerializeField] private List<string> parts; // Partes del texto dividido.

    private const string API_URL = "https://translate.google.com/translate_tts"; // URL de la API de Google.
    private const string LANG = "es"; // Idioma para la síntesis de texto.
    private const int MAX_LENGTH = 150; // Longitud máxima permitida para cada segmento.

    private Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>(); // Caché local para los clips descargados.

    private string GenerateUrl(string text, string lang)
    {
        var encodedText = UnityWebRequest.EscapeURL(text);
        return $"{API_URL}?ie=UTF-8&tl={lang}&client=tw-ob&q={encodedText}";
    }

    public void StartTTS(string text)
    {
        this.text = text;
        StopAllCoroutines(); // Detiene cualquier reproducción o descarga previa.
        StartCoroutine(IStartTTS());
    }

    public void StopTTS()
    {
        StopAllCoroutines();
        _audioSource.Stop();
    }

    private IEnumerator IStartTTS()
    {
        parts = SplitText(text);

        foreach (string part in parts)
        {
            if (audioCache.ContainsKey(part)) // Si el clip ya está en caché, se reproduce directamente.
            {
                _audioSource.clip = audioCache[part];
                _audioSource.Play();
                yield return new WaitForSeconds(_audioSource.clip.length - 0.3f);
                continue;
            }

            var url = GenerateUrl(part, LANG);
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error al descargar el audio: " + www.error);
                    continue;
                }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioCache[part] = clip; // Se almacena en la caché.

                _audioSource.clip = clip;
                _audioSource.Play();
                yield return new WaitForSeconds(clip.length - 0.3f);
            }
        }
    }

    private List<string> SplitText(string text, int maxLength = MAX_LENGTH)
    {
        List<string> parts = new List<string>();
        var sentences = Regex.Split(text, @"(?<=[.!?¿()]) +"); // Divide en oraciones.
        string currentPart = "";

        foreach (var sentence in sentences)
        {
            if ((currentPart + sentence).Length <= maxLength)
            {
                currentPart += sentence + " ";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(currentPart))
                    parts.Add(currentPart.Trim());

                currentPart = sentence + " ";
            }
        }

        if (!string.IsNullOrWhiteSpace(currentPart))
            parts.Add(currentPart.Trim());

        return parts;
    }
}

/*******************************************************************************************************************/
