// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using System.Collections;
using OscJack;

class ServerTest : MonoBehaviour
{
    IEnumerator Start()
    {
        //var server = new OscServer(9000); // Port number
        var server = new OscServer(9000, "225.6.7.8"); // Port number

        server.MessageDispatcher.AddCallback(
            "/test", // OSC address
            (string address, OscDataHandle data) =>
            {
                Debug.Log(string.Format("({0}, {1})",
                    data.GetElementAsFloat(0),
                    data.GetElementAsFloat(1)));
            }
        );


        server.MessageDispatcher.AddCallback("/test/bool", (string address, OscDataHandle data) =>
        {
            Debug.LogFormat("{0} {1}", address, data.GetElementAsBool(0));
        });

        server.MessageDispatcher.AddCallback("/sensor/skeleton", (string address, OscDataHandle data) =>
        {
            Debug.LogFormat("{0} {1} {2}", address,
                data.GetElementAsInt(0),
                data.GetElementAsFloat(1));
        });
        yield return new WaitForSeconds(15);
        server.Dispose();
    }
}