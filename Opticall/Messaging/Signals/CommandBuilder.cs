using System.Collections;
using System.Reflection;

namespace Opticall.Messaging.Signals;

public class CommandBuilder : ICommandBuilder
{
    private Dictionary<Type, ValuesTemplate> _getters;

    private class ValuesTemplate : IEnumerable<Tuple<int, PropertyInfo>>
    {
        private byte[] _template;
        IList<Tuple<int, PropertyInfo>> _getters;

        public ValuesTemplate(byte[] template)
        {
            _template = template;
            _getters = new List<Tuple<int, PropertyInfo>>();
        }

        public int Count { get { return _template.Length; }}

        public void Add(int position, PropertyInfo propertyInfo)
        {
            _getters.Add(new Tuple<int, PropertyInfo>(position, propertyInfo));
        }

        public byte[] GenerateTemplate()
        {
            var newtemplate = new byte[_template.Length];
            Array.Copy(_template, newtemplate, _template.Length);
            return newtemplate;
        }

        public IEnumerator<Tuple<int, PropertyInfo>> GetEnumerator()
        {
            return _getters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public CommandBuilder()
    {
        _getters = [];

        foreach(var type in typeof(CommandBuilder).Assembly.GetTypes())
        {
             var attribute = type.GetCustomAttribute<SignalAttribute>();

            if(attribute == null)
                continue;

            var templateAttribute = type.GetCustomAttribute<CommandTemplateAttribute>();

            if(templateAttribute == null)
                continue;

            var getters = new ValuesTemplate(templateAttribute.Template);

            foreach(var propertyInfo in type.GetProperties())
            {
                var cf = propertyInfo.GetCustomAttribute<CommandFieldAttribute>();

                if(cf == null)
                {
                    continue;
                }

                getters.Add(cf.Position, propertyInfo);
            }

            _getters.Add(type, getters);
        }
    }

    public byte[]? Build(ISignalTopic signal)
    {
        if(signal == null)
            return null;

        var signalType = signal.GetType();

        if(_getters.TryGetValue(signalType, out ValuesTemplate? valuesTemplate) && valuesTemplate != null)
        {
            var result = valuesTemplate.GenerateTemplate();

            foreach(var getter in valuesTemplate)
            {
                var propertyValue = getter.Item2.GetValue(signal);

                if(propertyValue != null)
                {
                    result[getter.Item1] = (byte)propertyValue;
                }
            }

            return result;
        }

        return null;
    }
}