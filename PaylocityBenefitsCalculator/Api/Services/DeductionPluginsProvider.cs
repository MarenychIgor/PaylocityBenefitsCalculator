using Api.Abstractions;

namespace Api.Services
{
    public class DeductionPluginsProvider : IDeductionPluginsProvider
    {
        // To achieve plug and play logic I'm instantiating objects for those classes which implements IDeductionPlugin.     
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
