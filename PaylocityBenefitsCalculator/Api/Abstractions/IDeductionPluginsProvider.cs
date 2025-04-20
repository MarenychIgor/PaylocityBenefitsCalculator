namespace Api.Abstractions
{
    public interface IDeductionPluginsProvider
    {
        IEnumerable<IDeductionPlugin> GetPlugins();
    }
}
