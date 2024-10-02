using DemiurgEngine.StatSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DemiurgEngine.StatSystem
{
    public class SaveLoad
    {
        List<StatsControllerCharacter> _controllers;
        string _key;

        public string SaveKey { get => _key; set => _key = value; }

        public void Init()
        {

        }
        public void Save()
        {
            if (_controllers.Count > 0)
            {
                //TODO
                //string data = Serializer.Serialize(_controllers.GetArray());

                //PlayerPrefs.SetString(_key + ".Stats", data);
            }
        }

        public void Load()
        {
            string data = PlayerPrefs.GetString(SaveKey + ".Stats");
            if (string.IsNullOrEmpty(data)) { return; }

            List<object> list = MiniJSON.Deserialize(data) as List<object>;

            for (int i = 0; i < list.Count; i++)
            {
                Dictionary<string, object> handlerData = list[i] as Dictionary<string, object>;
                string handlerName = (string)handlerData["Name"];

                //TODO
                //StatsController handler = _controllers.GetObject(handlerName);
                //if (handler != null)
                //{
                //    handler.SetObjectData(handlerData);
                //}
            }
        }
    }
}