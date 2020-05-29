using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class UserAuth : MonoBehaviour
{
    public string uuid = "";
    private const string USER = "User";

    private NCMBQuery<NCMBObject> queryUser;

    void Start()
    {
        uuid = PlayerPrefs.GetString(USER, "");
        Debug.Log("UUID: " + uuid);

        if (uuid.Equals(""))
        {
            UserRegistration();
        }
    }

    private void UserRegistration()
    {
        queryUser = new NCMBQuery<NCMBObject>(USER);

        queryUser.CountAsync((int count, NCMBException e) =>
        {
            if (e != null)
            {
                // TODO:
                Debug.Log("Userの登録に失敗しました.");
            }
            else
            {
                NCMBObject user = new NCMBObject(USER);

                user["ID"] = count + 1;

                user.SaveAsync((NCMBException ee) =>
                {
                    if (ee != null)
                    {
                        Debug.Log("bug");
                        Debug.Log(ee.ToString());
                    }
                    else
                    {
                        uuid = user.ObjectId;
                        PlayerPrefs.SetString(USER, uuid);
                        Debug.Log(uuid);
                    }
                });
            }
        });
    }
}
