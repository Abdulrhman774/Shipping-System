namespace BL.Mapping
{
    public class AutoMapper : IMapper
    {
        private readonly global::AutoMapper.IMapper _mapper;

        public AutoMapper(global::AutoMapper.IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return _mapper.Map<List<TDestination>>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }
    }
}
