namespace BL.Mapping
{
    public interface IBaseMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
        List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source); 
    }
}
