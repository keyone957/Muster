using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;

public class TopBar : MonoBehaviour
{
    [SerializeField] Button CloseButton;
    [SerializeField] Button WindowButton;
    [SerializeField] Button MinButton;
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    void Start()
    {
        CloseButton.onClick.AddListener(Close);
        WindowButton.onClick.AddListener(Window);
        MinButton.onClick.AddListener(Min);
    }

    public void Close()
    {
        #if UNITY_EDITOR
        #else
                Application.Quit();
        #endif
        
    }
    public void Window()
    {
        
    }
    public void Min()
    {
        #if UNITY_EDITOR
        #else
                ShowWindow(GetActiveWindow(), 2);
        #endif
    }
}
