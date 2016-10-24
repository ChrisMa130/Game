using System.Collections.Generic;

// 游戏的各类配置文件

public static class GameConfig
{
    // 书稿对应的文字描述
    public static Dictionary<int, string> DocsDesc = new Dictionary<int, string>()
    {
        {1, "这是第一本书的描述，嗯。。。最好不要太长，否则没办法塞满呀。。。"},
        {2, "这是第二本书的描述。这是第二本书的描述。这是第二本书的描述。这是第二本书的描述。这是第二本书的描述。"},
        {3, "这是第三本书的描述。这是第三本书的描述。这是第三本书的描述。这是第三本书的描述。"}
    };
}
