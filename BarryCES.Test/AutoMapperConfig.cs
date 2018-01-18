using AutoMapper;
using BarryCES.Infrastructure;
using BarryCES.Infrastructure.Extentions;
using BarryCES.Services;

namespace BarryCES.Test
{
    /// <summary>
    /// AutoMapperConfig
    /// </summary>
    public class AutoMapperConfig
    {
        private static MapperConfiguration _mapperConfiguration;

        /// <summary>
        /// 
        /// </summary>
        public static void Register()
        {
            var moduleInitializers = new ModuleInitializer[]
            {
                new BarryCESModuleInitializer()
            };

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                moduleInitializers.ForEach(m => m.LoadAutoMapper(cfg));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MapperConfiguration GetMapperConfiguration()
        {
            if(_mapperConfiguration == null)
                Register();

            return _mapperConfiguration;
        }
    }
}