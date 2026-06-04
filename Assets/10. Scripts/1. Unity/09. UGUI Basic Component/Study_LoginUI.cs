using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Study_LoginUI
{
    public class Study_LoginUI : MonoBehaviour
    {
        [field: SerializeField]
        private TMP_InputField EmailField { get; set; }

        [field: SerializeField]
        private TMP_InputField PasswordField { get; set; }

        [field: SerializeField]
        private Button LoginButton { get; set; }
    }

    public class User
    {
        public string ID { get; private set; }
        public string Password { get; private set; }

        public User(string id, string password)
        {
            ID = id;
            Password = password;
        }

        public override string ToString()
        {
            return $"User : ID = {ID}, PW = {Password}";
        }
    }
}


