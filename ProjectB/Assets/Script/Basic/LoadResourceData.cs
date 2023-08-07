using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadResourceData : MonoBehaviour
{
    public IEnumerator CheckResourceVersion()
    {
        var sizeHandler = Addressables.GetDownloadSizeAsync("Remote");
        if(sizeHandler.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
            ResourceLoadResult(false);

        while(sizeHandler.IsDone == false)
        {
            yield return new WaitForEndOfFrame();
        }

        if(sizeHandler.Result > 0)
        {
            var downloadHandle = Addressables.DownloadDependenciesAsync("Remote");
            if (downloadHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
                ResourceLoadResult(false);

            while(downloadHandle.IsDone == false)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        yield return null;
    }

    private void ResourceLoadResult(bool isSuc)
    {
        Debug.LogError("Resource Load Fail");
        StopAllCoroutines();

        if (isSuc)
        {
            //작업필요
            //성공 팝업
        }
        else
        {
            //작업필요
            //실패 팝업
        }
    }
}
