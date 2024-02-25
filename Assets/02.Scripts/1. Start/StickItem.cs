using System;
using System.Collections.Generic;

/// <summary>
/// 응원봉 정보 목록
/// </summary>
[Serializable]
public class StickList
{
    //응원봉 정보
    [Serializable]
    public class Item
    {
        public int _ID;
        public string _name;
        public string _MainText;
        public string _subText;
    }

    public List<Item> _items = new List<Item>();
}