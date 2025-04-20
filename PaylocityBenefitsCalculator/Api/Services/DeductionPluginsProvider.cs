using Api.Abstractions;

namespace Api.Services
{
    public class DeductionPluginsProvider : IDeductionPluginsProvider
    {
        public IEnumerable<IDeductionPlugin> GetPlugins()
        {
            var result = new List<IDeductionPlugin>();
            
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IDeductionPlugin).IsAssignableFrom(x) && x.IsClass);

            foreach(var type in types)
                result.Add(Activator.CreateInstance(type) as IDeductionPlugin);

            return result;
        }
    }
}
