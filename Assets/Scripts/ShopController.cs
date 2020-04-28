using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public Transform content;
    public Button cancelBtn;
    public List<Button> shopOfferBtn;
    public List<ShopOffer> shopOffers;
    AudioSource sounds;
    private void Start()
    {
        sounds = GameObject.Find("ButtonSound_0(Clone)").GetComponent<AudioSource>();

        shopOffers = new List<ShopOffer>();
        cancelBtn.onClick.AddListener(() =>
        {
            Globals.session.level++;
            Destroy(gameObject);
        });

        shopOffers.Add(new ShopOffer(ShopOfferType.ExtraSpeed, 50));
        shopOffers.Add(new ShopOffer(ShopOfferType.ExtraBullet, 75));
        shopOffers.Add(new ShopOffer(ShopOfferType.LessSpeed, 150));
        shopOffers.Add(new ShopOffer(ShopOfferType.AutoFire, 400));
        shopOffers.Add(new ShopOffer(ShopOfferType.ExtraLife, 800));
        shopOffers.Add(new ShopOffer(ShopOfferType.Armor, 600));
        shopOffers.Add(new ShopOffer(ShopOfferType.ExtraTime, 1500));
        shopOffers.Add(new ShopOffer(ShopOfferType.DoubleShoot, 100));
        shopOffers.Add(new ShopOffer(ShopOfferType.TripleShoot, 300));
        shopOffers.Add(new ShopOffer(ShopOfferType.FourShoot, 400));

        for (int i = 0; i < content.childCount; i++)
        {
            shopOfferBtn.Add(content.GetChild(i).GetComponent<Button>());
            shopOfferBtn[i].transform.Find("Price").GetComponent<Text>().text = shopOffers[i].price + " Z";
        }

        shopOfferBtn[0].onClick.AddListener(() => BuySomething(ShopOfferType.ExtraSpeed));
        shopOfferBtn[1].onClick.AddListener(() => BuySomething(ShopOfferType.ExtraBullet));
        shopOfferBtn[2].onClick.AddListener(() => BuySomething(ShopOfferType.LessSpeed));
        shopOfferBtn[3].onClick.AddListener(() => BuySomething(ShopOfferType.AutoFire));
        shopOfferBtn[4].onClick.AddListener(() => BuySomething(ShopOfferType.ExtraLife));
        shopOfferBtn[5].onClick.AddListener(() => BuySomething(ShopOfferType.Armor));
        shopOfferBtn[6].onClick.AddListener(() => BuySomething(ShopOfferType.ExtraTime));
        shopOfferBtn[7].onClick.AddListener(() => BuySomething(ShopOfferType.DoubleShoot));
        shopOfferBtn[8].onClick.AddListener(() => BuySomething(ShopOfferType.TripleShoot));
        shopOfferBtn[9].onClick.AddListener(() => BuySomething(ShopOfferType.FourShoot));
    }

    private void Update()
    {
        for (int i = 0; i < shopOffers.Count; i++)
            if (!shopOffers[i].isAvailable)
                shopOfferBtn[i].interactable = false;
    }

    public void BuySomething(ShopOfferType shopOfferType)
    {
        if (Globals.game.isSounds)
            sounds.Play();

        Globals.session.cash -= shopOffers.Where(x => x.shopOfferType == shopOfferType).FirstOrDefault().price;

        switch (shopOfferType)
        {
            case ShopOfferType.ExtraSpeed:
                Globals.session.movemementSpeed++;
                break;
            case ShopOfferType.ExtraBullet:
                Globals.session.fireSpeed++;
                break;
            case ShopOfferType.LessSpeed:
                Globals.session.movemementSpeed--;
                break;
            case ShopOfferType.AutoFire:
                Globals.session.autoFire = true;
                break;
            case ShopOfferType.ExtraLife:
                Globals.session.lifes++;
                break;
            case ShopOfferType.Armor:
                Globals.session.armor++;
                break;
            case ShopOfferType.ExtraTime:
                Globals.session.time += 30;
                break;
            case ShopOfferType.DoubleShoot:
                Globals.session.weaponType = BulletType.Double_Green;
                break;
            case ShopOfferType.TripleShoot:
                Globals.session.weaponType = BulletType.Triple_Blue;
                break;
            case ShopOfferType.FourShoot:
                Globals.session.weaponType = BulletType.Four_Red;
                break;
        }
    }
}

public class ShopOffer
{
    public ShopOfferType shopOfferType;
    public int price;
    public bool isAvailable
    {
        get
        {
            if (price > Globals.session.cash)
                return false;

            switch (shopOfferType)
            {
                case ShopOfferType.ExtraSpeed:
                    return Globals.session.movemementSpeed < 50;
                case ShopOfferType.LessSpeed:
                    return Globals.session.movemementSpeed > 1;
                case ShopOfferType.ExtraBullet:
                    return Globals.session.fireSpeed < 50;
                case ShopOfferType.ExtraLife:
                    return Globals.session.lifes < 3;
                case ShopOfferType.Armor:
                    return Globals.session.armor < 3;
                case ShopOfferType.AutoFire:
                    return !Globals.session.autoFire;
                default:
                    return false;
            };
        }
    }

    public ShopOffer(ShopOfferType shopOfferType, int price)
    {
        this.shopOfferType = shopOfferType;
        this.price = price;
    }
}
public enum ShopOfferType
{
    ExtraSpeed,
    LessSpeed,
    ExtraBullet,
    ExtraLife,
    ExtraTime,
    Armor,
    DoubleShoot,
    TripleShoot,
    FourShoot,
    AutoFire
}
