using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRegister : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField passwordConfirm;
    public Button buttonRegister;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        if (string.IsNullOrEmpty (this.passwordConfirm.text))
        {
            MessageBox.Show("��ȷ������");
            return;
        }
        if (this.password.text != this.passwordConfirm.text)
        {
            MessageBox.Show("������������벻һ��");
            return;
        }
    }
}
