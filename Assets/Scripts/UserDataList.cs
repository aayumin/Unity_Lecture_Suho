using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class UserDataList
{
    public List<UserData> users;
    // Start is called before the first frame update

    public UserDataList() {
        users = new List<UserData>();
    }

  

    public void DeleteUser(string nickname){
        foreach(UserData user in users){
            if (user.nickname == nickname) {
                users.Remove(user);
                break;
            }
        }
    }

    public void AddUser(string nickname, int level){
        UserData temp = new UserData();
        temp.SetUsername(nickname);
        users.Add(temp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
