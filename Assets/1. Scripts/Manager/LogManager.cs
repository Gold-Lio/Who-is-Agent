using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class LogManager
{
    static public string s_LogPath = "";

    public static void SetLogPath()
    {
        // 폴더이름
        string t_Directory = "/Log";

        // 로그 파일 이름
        string t_FileName = "Log.txt";

        // 폴더 경로를 저장하기 위한 변수 (해당 폴더가 없는 경우 생성하기 위함)
        string t_Path = "";

        // 아이폰인 경우 폴더 경로 및 파일명이 포함된 경로 생성
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // ios 테스트 못함
            t_Path = Path.Combine(Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 5), "Documents"), "");
            s_LogPath = Path.Combine(Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 5), "Documents/"), t_FileName);
        }
        // 안드로이드인 경우 폴더 경로 및 파일명이 포함된 경로 생성
        else if (Application.platform == RuntimePlatform.Android)
        {
            t_Path = Application.persistentDataPath + t_Directory;
            s_LogPath = Application.persistentDataPath + t_Directory + "/" + t_FileName;
        }
        // 기타 운영 체제(여기서는 유니티)
        else
        {
            t_Path = (Application.dataPath + t_Directory);
            // 해당 프로그램(유니티) 폴더 하위에 생성 (폴더 생성 가능)
            s_LogPath = (Application.dataPath + t_Directory + "/" + t_FileName);
        }

        // 폴더 경로를 확인(열어보고) 후 없다면 폴더를 생성
        if (!Directory.Exists(t_Path))
        {
            Directory.CreateDirectory(t_Path);
        }
    }

    public static void Log(int num)
    {
        Log(num.ToString());
    }

    public static void Log(float num)
    {
        Log(num.ToString());
    }

    public static void Log(double num)
    {
        Log(num.ToString());
    }

    public static void Log(bool isbool)
    {
        Log(isbool.ToString());
    }

    public static void Log(string _logmsg)
    {
        // 시스템 로그도 기록
        Debug.Log(_logmsg);

        // 파일 경로가 만들어지지 않은 상태라면 파일경로 생성 함수(메소드)로 생성
        if (s_LogPath == "")
        {
            SetLogPath();
        }

        FileStream t_File = null;

        // 파일 확인하고 없다면 파일을 생성
        if (!File.Exists(s_LogPath))
        {
            //File.Create(s_LogPath);
            t_File = new FileStream(s_LogPath, FileMode.Create, FileAccess.Write);
        }
        // 파일이 있드면 내용 추가 형식으로 열기
        else
        {
            t_File = new FileStream(s_LogPath, FileMode.Append);
        }

        // 열린 파일이 크기이 크다면 닫고 새 파일스트림으로 생성
        if (t_File.Length > 1048000)
        {
            t_File.Close();
            t_File = new FileStream(s_LogPath, FileMode.Create, FileAccess.Write);
        }

        StreamWriter t_SW = new StreamWriter(t_File);

        // 로그 내용 앞에 시간 추가 
        string t_Logfrm = DateTime.Now.ToString("MM-dd hh:mm:ss") + " -- " + _logmsg;

        // 로그 기록
        t_SW.WriteLine(t_Logfrm);

        // 사용했던 스트림들 닫기
        t_SW.Close();
        t_File.Close();
    }
}
