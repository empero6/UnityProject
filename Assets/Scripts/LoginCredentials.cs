using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VivoxUnity;
using System.ComponentModel;
using System;

public class LoginCredentials : MonoBehaviour
{
    VivoxUnity.Client client;
    private Uri server = new Uri("");
    private string issuer = "";
    private string domain = "";
    private string tokenKey = "";
    private TimeSpan timeSpan = new TimeSpan(90);

    private ILoginSession loginSession;

    private void Awake() 
    {
        client = new Client();
        client.Uninitialize();
        client.Initialize();
        DontDestroyOnLoad(this);
    }

    private void OnApplicationQuit()
    {
        client.Uninitialize();

    }

    void Start()
    {

    }

    public void Bind_Login_Callback_Listeners(bool bind, ILoginSession loginSess) 
    {
        if (bind)
        {
            loginSess.PropertyChanged += Login_Status;

        }
        else
        {
            loginSess.PropertyChanged -= Login_Status;

        }
    }

    public void Login(string userName) 
    {
        AccountId accountId = new AccountId(issuer, userName, domain);
        Bind_Login_Callback_Listeners(true, loginSession);
        loginSession.BeginLogin(server, loginSession.GetLoginToken(tokenKey, timeSpan), ar => {
            try 
            {
                loginSession.EndLogin(ar);
            }
            catch (Exception e)
            {
                Bind_Login_Callback_Listeners(false, loginSession);
                Debug.Log(e.Message);
            }
        });
    }

    // public AsyncCallback v;

    // private void Login_Result(IAsyncResult asyncResult) 
    // {
    //     try 
    //     {
    //         loginSession.EndLogin(ar);
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.log(e.Message);
    //     }
    // }

    private void Logout()
    {
        loginSession.Logout();
        Bind_Login_Callback_Listeners(false, loginSession);
    }

    void Login_Status(object sender, PropertyChangedEventArgs loginArgs)
    {
        var source = (ILoginSession)sender;

        switch(source.State)
        {
            case LoginState.LoggingIn:
                Debug.Log("Logging in");
                break;
            
            case LoginState.LoggedIn:
                Debug.Log($"Logged in ");
                break;
        }
    }
}
