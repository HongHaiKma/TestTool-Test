using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

public class LoadingDependencies : MonoBehaviour
{
    public TextMeshProUGUI txt_Percent;
    public Button btn_Loading;
    public Image img_LoadImg;
    public Texture2D texture2D;

    private AsyncOperationHandle loadImg;

    private void Start()
    {
        btn_Loading.onClick.AddListener(() => LoadingDependency());
    }

    // private void Update()
    // {
    //     txt_Percent.text = loadImg.PercentComplete.ToString() + $"%";
    // }

    void LoadingDependency()
    {
        loadImg = Addressables.DownloadDependenciesAsync("darts ninja_1");
        // loadImg.Completed += (OnImgLoaded) =>
        // {
        //     Debug.Log("!!!!!!!!!!!!!!!!!!!!!");
        // };

        loadImg.Completed += OnImgLoaded;
    }

    // void OnImgLoaded(AsyncOperationHandle<Texture2D> obj)
    void OnImgLoaded(AsyncOperationHandle obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("ADDRESABLE LOAD SUCCESS");
            // obj.Convert<Texture2D>();
            Debug.Log(obj.Result);
            // Sprite result = obj.Convert<Sprite>().Result;
            // texture2D = result;
            // img_LoadImg.sprite = result;
        }
        else
        {
            Debug.Log("FAILEDDDDDDDDDDDDDDDDDD");
        }
    }
}
