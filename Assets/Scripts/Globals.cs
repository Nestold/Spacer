using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Advertisements;

public class Globals : MonoBehaviour
{
    public static Game game;
    public static Session session;
    public static AdsController adsController;
    public static System.Random rand;
    
    public static string googleId = "3560531";
    public static bool TestMode = true;
    public static string placementId = "rewardedVideo";

    public static bool respawnPlayer = false;
    public static void LoadData()
    {
        if(rand == null && game == null)
        {
            rand = new System.Random();
            game = new Game();
            game.Load();
            adsController = new AdsController();
        }
    }

    public static void CreateBullet(BulletType bulletType, Transform shooter)
    {
        Transform pom; 
        if(shooter.tag.Equals("Player"))
            pom = shooter.parent;
        else
            pom = shooter.parent.parent;

        switch (bulletType)
        {
            case BulletType.One_Blue:
                var bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = shooter.localPosition;
                bullet.GetComponent<ShootController>().Init(shooter.tag);
                break;

            case BulletType.One_Yellow:
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = shooter.localPosition;
                bullet.GetComponent<ShootController>().Init(shooter.tag);
                break;

            case BulletType.Double_Green:
                var pos = shooter.localPosition;
                pos.x += 0.15f;
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = pos;
                bullet.GetComponent<ShootController>().Init(shooter.tag);

                pos.x -= 0.3f;
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = pos;
                bullet.GetComponent<ShootController>().Init(shooter.tag);
                break;

            case BulletType.Double_Yellow:
                pos = shooter.localPosition;
                pos.x += 0.15f;
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = pos;
                bullet.GetComponent<ShootController>().Init(shooter.tag);

                pos.x -= 0.3f;
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = pos;
                bullet.GetComponent<ShootController>().Init(shooter.tag);
                break;

            case BulletType.Triple_Blue:
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = shooter.localPosition;
                bullet.GetComponent<ShootController>().Init(shooter.tag);

                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = shooter.localPosition;
                bullet.GetComponent<ShootController>().Init(shooter.tag, 1);

                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = shooter.localPosition;
                bullet.GetComponent<ShootController>().Init(shooter.tag, -1);
                break;

            case BulletType.Triple_Yellow:
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = shooter.localPosition;
                bullet.GetComponent<ShootController>().Init(shooter.tag);

                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = shooter.localPosition;
                bullet.GetComponent<ShootController>().Init(shooter.tag, 1);

                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = shooter.localPosition;
                bullet.GetComponent<ShootController>().Init(shooter.tag, -1);
                break;

            case BulletType.Four_Red:
                pos = shooter.localPosition;

                pos.x -= 0.45f;
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = pos;
                bullet.GetComponent<ShootController>().Init(shooter.tag);
                pos = shooter.localPosition;

                pos.x -= 0.15f;
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = pos;
                bullet.GetComponent<ShootController>().Init(shooter.tag);
                pos = shooter.localPosition;

                pos.x += 0.15f;
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = pos;
                bullet.GetComponent<ShootController>().Init(shooter.tag);
                pos = shooter.localPosition;

                pos.x += 0.45f;
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Bullet_Blue"), pom, false);
                bullet.transform.localPosition = pos;
                bullet.GetComponent<ShootController>().Init(shooter.tag);
                break;
        }
    }
    public static byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);

        return ms.ToArray();
    }
    public static object ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        object obj = (object)binForm.Deserialize(memStream);

        return obj;
    }
    public static string FromByteArrayToString(byte[] obj)
    {
        string result = "";
        foreach (byte b in obj)
        {
            result += b.ToString("X");
            result += " ";
        }
        return result;
    }
    public static byte[] FromStringBytearray(string value)
    {
        List<byte> result = new List<byte>();
        string word = "";
        foreach (char c in value)
        {
            if (c.Equals(' '))
            {
                result.Add(byte.Parse(word, System.Globalization.NumberStyles.HexNumber));
                word = "";
                continue;
            }
            else
            {
                word += c;
            }
        }
        return result.ToArray();
    }
    public static string GetNumWithDots(ulong number)
    {
        string score = number.ToString("N1", CultureInfo.InvariantCulture);
        string result = "";

        foreach (char c in score)
        {
            if (c != '.')
                result += c;
            else if (c == ',')
                result += '.';
            else
                break;
        }
        return result;
    }
    public static string GetNumWithDots(int number)
    {
        return GetNumWithDots((ulong)number);
    }
    public static void CreateBlure(BlureType blureType, Transform parent, string sceneName = "")
    {
        switch(blureType)
        {
            case BlureType.In:
                var window = Instantiate(Resources.Load<GameObject>("Prefabs/UI/BlureWindow"), parent, false);
                window.GetComponent<BlureController>().In();
                break;

            case BlureType.Out:
                window = Instantiate(Resources.Load<GameObject>("Prefabs/UI/BlureWindow"), parent, false);
                window.GetComponent<BlureController>().Out(sceneName);
                break;
        }
    }
}

public enum BulletType
{
        //Player
    One_Blue,
    Double_Green,
    Triple_Blue,
    Four_Red,
        //Enemy
    One_Yellow,
    Double_Yellow,
    Triple_Yellow
}
[Serializable]
public class Game
{
    public Player player;
    string filePath;

    public bool isMusic;
    public bool isSounds;

    public Game()
    {
        player = new Player();
        filePath = Application.persistentDataPath + "/game.data";
        isMusic = true;
        isSounds = true;
    }

    public void Save()
    {
        Debug.Log("Save to: " + filePath);
        using (StreamWriter sw = new StreamWriter(File.Open(filePath, FileMode.OpenOrCreate)))
        {
            sw.WriteLine(Globals.FromByteArrayToString(Globals.ObjectToByteArray(this)));
        }
    }
    public void Load()
    {
        if (!File.Exists(filePath))
            Save();
        Debug.Log("Load from: " + filePath);
        using (StreamReader sr = new StreamReader(File.Open(filePath, FileMode.OpenOrCreate)))
        {
            var pom = (Game)Globals.ByteArrayToObject(Globals.FromStringBytearray(sr.ReadLine()));
            player = pom.player;
            isSounds = pom.isSounds;
            isMusic = pom.isMusic;
        }
    }
}
[Serializable]
public class Player
{
    public string name;
    public ulong[] highscore;
    public int shipID;
    public Player()
    {
        name = "";
        shipID = 0;
        highscore = new ulong[10];
    }

    internal void AddHighscore(ulong score)
    {
        for (int i = 0; i < highscore.Length; i++)
        {
            if (score > highscore[i])
            {
                if (i + 1 < highscore.Length)
                    highscore[i + 1] = highscore[i];
                highscore[i] = score;
                break;
            }
            else if(highscore[i] == 0)
            {
                highscore[i] = score;
                break;
            }
        }
    }
}

public class Session
{
    public int lifes;
    public int armor;
    public int fireSpeed;
    public int movemementSpeed;
    public bool autoFire;
    public bool[] extraLife;
    public ulong score;
    public int actualEnemySet;
    public bool adIsUsed;
    public BulletType weaponType;
    public int level;
    public int cash;
    public int time;

    public Session()
    {
        lifes = 1;
        armor = 0;
        fireSpeed = 1;
        movemementSpeed = 1;
        actualEnemySet = 0;
        autoFire = false;
        extraLife = new bool[] { false, false, false, false, false };
        score = 0;
        weaponType = BulletType.One_Blue;
        level = 1;
        adIsUsed = false;
        cash = 0;
        time = 180;
    }

    public float GetFireSpeed()
    {
        return 0.6f - (fireSpeed * 0.01f);
    }
    internal float GetMovementSpeed()
    {
        return 3f + (movemementSpeed / 2f);
    }
    public void AddLife()
    {
        if (lifes < 3)
            lifes++;
    }
    internal bool IsAutoFire()
    {
        return autoFire;
    }
    public bool UnLife()
    {
        if ((lifes - 1) > 0)
        {
            lifes--;
            return true;
        }
        else
        {
            lifes--;
            return false;
        }
    }
    internal void UnArmor()
    {
        armor--;
    }
}
