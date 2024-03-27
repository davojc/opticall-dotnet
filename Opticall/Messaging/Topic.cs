using System.Reflection;

namespace Opticall.Messaging;

public static class Topic
{
    public static string GetTopic<T>()
    {
        var topicAttribute = typeof(T).GetCustomAttribute<TopicAttribute>();

        return topicAttribute != null ? topicAttribute.Topic : string.Empty;
    }
}
