using System;
using System.Collections;
using UnityEngine;
namespace ExtraGameModes.HelperScripts
{
    static class FileLoader
    {
        //PUBLIC LOADING METHODS
        //Thanks Baan/GentleLeviathan!!!!
        public static GameObject GetAssetBundleAsGameObject(string path, string name)
        {
            Debug.Log("AssetBundleLoader: Attempting to load AssetBundle...");
            AssetBundle bundle = null;
            try
            {
                bundle = AssetBundle.LoadFromFile(path);
                Debug.Log("AssetBundleLoader: Success.");
            }
            catch (Exception e)
            {
                Debug.Log("AssetBundleLoader: Couldn't load AssetBundle from path: '" + path +
                          "'. Exception details: e: " + e.Message);
            }

            Debug.Log("AssetBundleLoader: Attempting to retrieve: '" + name + "' as type: 'GameObject'.");
            try
            {
                var temp = bundle.LoadAsset(name, typeof(GameObject));
                Debug.Log("AssetBundleLoader: Success.");
                return (GameObject) temp;
            }
            catch (Exception e)
            {
                
                Debug.Log("AssetBundleLoader: Couldn't retrieve GameObject from AssetBundle.");
                return null;
            }
        }
    }
}