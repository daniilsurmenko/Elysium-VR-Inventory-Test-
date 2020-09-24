using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class SendRequestManager : MonoBehaviour
{
    public UnityEvent OnSendRequest;

    [HideInInspector] public string Identificator;
    [HideInInspector] public string EventName;
    string key = "auth";
    string keyValue = "BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6";
    string url = "https://dev3r02.elysium.today/inventory/status/";
    public void SendPostRequest()
    {
        StartCoroutine(SendPR());
    }
    public IEnumerator SendPR()
    {
        WWWForm formData = new WWWForm();

        formData.AddField("event-name", EventName);// добавляем поля с их значениями
        formData.AddField("identificator", Identificator);

        UnityWebRequest request = UnityWebRequest.Post(url, formData);// создаём запрос

        request.SetRequestHeader(key, keyValue);// авторизируемся

        yield return request.SendWebRequest();

        Debug.Log(Identificator);
        Debug.Log(EventName);
        Debug.Log(request.downloadHandler.text);


    }
}
