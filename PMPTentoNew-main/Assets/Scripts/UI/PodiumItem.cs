using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class PodiumItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _username;
    [SerializeField] private TextMeshProUGUI _totalExperience;
    [SerializeField] private SVGImage _imageAvatar;//hay que reemplazar por SVImage para el avatar
    [SerializeField] private TextMeshProUGUI _position;

    private int _id;
    public void SetData(string position, string username, string totalExperience, int id, Sprite sprite)//se usa para el resto de posiciones
    {
        _position.text = position;
        _username.text = username;
        _totalExperience.text = totalExperience;
        _id = id;
        _imageAvatar.sprite = sprite;
    }
    public void SetDataPodio(string username, string totalExperience, int id, Sprite sprite)//se usa para el podio, las 3 primeras posiciones
    {
        _username.text = username;
        _totalExperience.text = totalExperience;
        _id = id;
        _imageAvatar.sprite = sprite;
    }

}
