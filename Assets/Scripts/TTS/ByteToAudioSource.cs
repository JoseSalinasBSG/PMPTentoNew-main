using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.IO;
using System.Text.RegularExpressions;
// using NAudio.Wave;
// using TTS;
using UnityEngine.Networking;

public class ByteToAudioSource : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private string text;
    [SerializeField] private List<string> parts;

    private const string API_URL = "https://translate.google.com/translate_tts";
    private const string LANG = "es";
    private const int MAX_LENGTH = 200;
    
    private string GenerateUrl(string text, string lang)
    {
        var encodedText = UnityWebRequest.EscapeURL(text);
        return $"{API_URL}?ie=UTF-8&tl={lang}&client=tw-ob&q={encodedText}";
    }

    public void StartTTS(string text)
    {
        this.text = text;
        StartCoroutine(IStartTTS());
    }

    public void StopTTS()
    {
        StopAllCoroutines();
        _audioSource.Stop();
    }
    private IEnumerator IStartTTS()
    {
        yield return new WaitForSeconds(.03f);
        parts = SplitText(text);
        foreach (string part in parts)
        {
            var url = GenerateUrl(part, LANG);
            //Debug.Log(url);
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                yield return www.SendWebRequest();
    
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error al descargar el audio: " + www.error);
                }
                else
                {
                    // byte[] audioData = DownloadHandlerAudioClip.GetContent(www)/*www.downloadHandler.data;*/
                    _audioSource.clip = DownloadHandlerAudioClip.GetContent(www);/*www.downloadHandler.data;*/
                    _audioSource.Play();
                    // PlayMP3(audioData);
                    yield return new WaitForSeconds(_audioSource.clip.length);
                }
            }

            yield return null;

        }
    }
    
    private List<string> SplitText(string text, int maxLength = MAX_LENGTH)
    {
        List<string> parts = new List<string>();
        var sentences = Regex.Split(text, @"(?<=[.!?]) +");
        for (int i = 0; i < sentences.Length; i++)
        {
            Debug.Log(sentences[i]);
            if (sentences[i].Length <= maxLength)
            {
                parts.Add(sentences[i]);
            }
            else
            {
                // Debug.Log("Texto demasiado grande para procesar");
                // Debug.Log("Dividiendo texto");
                parts.AddRange(SplitLongSentence(sentences[i], 200));
                // Debug.Log(sentences[i].Length);
            }
        }
        return parts;
    }

    private List<string> SplitLongSentence(string sentence, int chunkSize)
    {
        List<string> chunks = new List<string>();
        if (sentence.Length <= chunkSize)
        {
            chunks.Add(sentence);
        }
        else
        {
            int lastSpaceIndex = sentence.LastIndexOf(' ', chunkSize);
            if (lastSpaceIndex == -1)
            {
                lastSpaceIndex = chunkSize;
            }
            string firstPart = sentence.Substring(0, lastSpaceIndex);
            string secondPart = sentence.Substring(lastSpaceIndex).TrimStart();
            
            chunks.Add(firstPart);
            chunks.AddRange(SplitLongSentence(secondPart, chunkSize));
        }
        return chunks;
    }
    
    // public void PlayMP3(byte[] mp3Bytes)
    // {
    //     using (MemoryStream mp3Stream = new MemoryStream(mp3Bytes))
    //     {
    //         using (Mp3FileReader mp3Reader = new Mp3FileReader(mp3Stream))
    //         {
    //             using (WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(mp3Reader))
    //             {
    //                 byte[] waveBytes = ReadFully(mp3Stream);
    //
    //                 AudioClip audioClip = WavUtility.ToAudioClip(waveBytes, 0, waveBytes.Length, 23500);
    //
    //                 _audioSource.clip = audioClip;
    //                 _audioSource.Play();
    //             }
    //         }
    //     }
    // }
    // private byte[] ReadFully(WaveStream waveStream)
    // {
    //     byte[] buffer = new byte[waveStream.Length];
    //     using (MemoryStream ms = new MemoryStream())
    //     {
    //         int bytesRead;
    //         while ((bytesRead = waveStream.Read(buffer, 0, buffer.Length)) > 0)
    //         {
    //             ms.Write(buffer, 0, bytesRead);
    //         }
    //         return ms.ToArray();
    //     }
    // }
}
