using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public Button buttonLogin;
    public Toggle aggreToS;
    public Toggle rememberUsername;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("�������˺�");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("����������");
            return;
        }
        if (!this.aggreToS.isOn)
        {
            MessageBox.Show("���Ķ���ͬ���û�Э��");
            return;
        }
        UserService.Instance.SendLogin(this.username.text, this.password.text);
    }
}
