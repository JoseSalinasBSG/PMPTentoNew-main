using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UI.CommentBox
{
    public class CommentItem : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI _nameUser;
        [SerializeField] private TextMeshProUGUI _messageUser;
        [SerializeField] private TextMeshProUGUI _dateComment;
        [SerializeField] private RawImage _imageUser;

        private DateTime _dateTime = new DateTime();
        
        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion

        #region Methods

        public void SetData(string nameUser, string messageUser, string imageUser, long dateComment)
        {
            _nameUser.text = nameUser;
            _messageUser.text = messageUser;
            _dateTime = new DateTime(dateComment);
            _dateComment.text =$"{_dateTime.Day} de {Utils.Constants.Months[_dateTime.Month]} {_dateTime.Hour}:{_dateTime.Minute}";
            StartCoroutine(ChargeImage(imageUser));
        }

        IEnumerator ChargeImage(string urlAvatar)
        {
            //Debug.LogWarning(urlAvatar);
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(urlAvatar);
            yield return request.SendWebRequest();

            if (request.responseCode <400)
            {
                try
                {
                    Debug.Log("success");

                    Texture2D texture2D = DownloadHandlerTexture.GetContent(request);
                    _imageUser.texture = texture2D;
                    Debug.Log("success2");

                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    throw;
                }
                
            }
            else
            {
                Debug.Log("error");
            }

        }
        #endregion

    }

}