using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;


//This class is going to be used for authenticating a user into UGS
//We will stop using individual people hosting, and do the networking
//and lobbying through Unity game Services.

//This class is static so any class has access to this class to do
//authentication at anytime.
public static class AuthenticationWrapper
{
    public static AuthState AuthState{get; private set;} = AuthState.NotAuthenticated;

    public static async Task<AuthState> DoAuth(int maxRetries = 5)
    {
        if(AuthState == AuthState.Authenticated)
        {
            return AuthState;
        }

        if(AuthState == AuthState.Authenticating){
            Debug.LogWarning("Already Authenticating");
            await Authenticating();
            return AuthState;
        }

        await SignInAnonymouslyAsync(maxRetries);

        return AuthState;
    }

    private static async Task<AuthState> Authenticating(){
        while(AuthState == AuthState.Authenticating || AuthState == AuthState.NotAuthenticated){
            await Task.Delay(200);
        }

        return AuthState;
    }

    private static async Task SignInAnonymouslyAsync(int maxRetries){
        AuthState = AuthState.Authenticating;
        int retries = 0;
        while(AuthState == AuthState.Authenticating && retries < maxRetries){
            //This is where you can edit ways that you can authenticate a player through
            //Unity netcode authentication service. For this project it is going to be
            //Anonymous since we don't need authentication but this is the too l to use
            //if I want to publish.

            //The sign in anonymously is good for mobile games where you don't want to
            //enter an identity and bind later or for prototyping.
            try{
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if(AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized){
                    AuthState = AuthState.Authenticated;
                    break;
                }
            }catch(AuthenticationException ex){
                Debug.LogError(ex);
                AuthState = AuthState.Error;
            }catch(RequestFailedException exception){
                Debug.LogError(exception);
                AuthState = AuthState.Error;
            }

            retries++;

            //If the authentication failed from the above wait 1 second before retrying
            await Task.Delay(1000);
        }

        if(AuthState != AuthState.Authenticated){
            Debug.LogWarning($"Player was not logged in successfully after {retries} retries");
            AuthState = AuthState.TimeOut;
        }
    }
}

public enum AuthState{
    NotAuthenticated,
    Authenticated,
    Authenticating,
    Error,
    TimeOut
}
