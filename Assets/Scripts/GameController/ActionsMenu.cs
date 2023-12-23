using UnityEditor;
using UnityEngine;

public static class ActionsMenu
{
#if UNITY_EDITOR

    #region Save Management
    [MenuItem("Actions/Remove Save", priority = 1)]
    private static void RemoveSave()
    {
        PlayerPrefs.DeleteAll();

        Serializer.DeleteFileAtPDP("save");
    }

    [MenuItem("Actions/Remove Save", true)]
    private static bool RemoveSaveValidation()
    {
        return !Application.isPlaying;
    }
    #endregion
#endif
}