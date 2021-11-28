using UnityEngine;

namespace PixelCrew.Utils.Disposables
{
    public static class WindowUtils
    {
        public static void CreateWindow(string resourcePath)
        {
            var window = Resources.Load<GameObject>(resourcePath);
            var canvas = Object.FindObjectOfType<WindowContainer>();
            Object.Instantiate(window, canvas.transform);
        }
    }
}