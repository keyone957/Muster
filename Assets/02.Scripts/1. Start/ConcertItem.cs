using System;
using System.Collections.Generic;

/// <summary>
/// 공연 정보 목록
/// </summary>
[Serializable]
public class ConcertList
{
    [Serializable]
    public class Info
    {
        public int _ID;
        public string _title;
        public string _name;
        public string _posterPath;
        public int _maxPeoples;
        public int _nowPeoples;
        public string _starTime;
    }

    public List<Info> _infos = new List<Info>();
}