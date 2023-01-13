//using AutoMapper;
using Mapster;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Service
{
    public static class ObjectMapper
    {

        #region AutoMapper
        //private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.AddProfile<DtoMapper>();
        //    });

        //    return config.CreateMapper();
        //});

        //public static IMapper Mapper => lazy.Value;
        #endregion


        #region Mapster
        //automapper i�indeki "ObjectMapper.Mapper.Map<TEntity>(dto)" kullan�m� de�i�mesin diye Mapper class i�ine al�nd�. 
        public static class Mapper  
        {
            public static TDestination Map<TDestination>( object source)
            {
                return source.Adapt<TDestination>(TypeAdapterConfig.GlobalSettings);
            }
        }
        #endregion
    }
}






//Benzer ama farkl� bir kurumda �al��maya ba�lad���m� payla�maktan mutluluk duyuyorum!
//Umar�m g�zel ve faydal� �al��malar �retebilirim.