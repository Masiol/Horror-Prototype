using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableLoader : MonoBehaviour
{
    [SerializeField] private Vector3 startPlayerPos;
    public static bool loadedAssets;
    private void Awake()
    {
        AsyncOperationHandle<GameObject> asyncOperationHandle = 
            Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Player.prefab");

        asyncOperationHandle.Completed += AsyncOperationHandle_Completed;
    }

    private void AsyncOperationHandle_Completed(AsyncOperationHandle<GameObject> asyncOperationHandle)
    {
        if(asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(asyncOperationHandle.Result, startPlayerPos, Quaternion.identity);

            loadedAssets = true;
        }
        else
        {
            Debug.Log("Failed");
        }
    }

}
