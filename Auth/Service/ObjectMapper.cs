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
        //automapper içindeki "ObjectMapper.Mapper.Map<TEntity>(dto)" kullanýmý deðiþmesin diye Mapper class içine alýndý. 
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






//Benzer ama farklý bir kurumda çalýþmaya baþladýðýmý paylaþmaktan mutluluk duyuyorum!
//Umarým güzel ve faydalý çalýþmalar üretebilirim.