using Library.Data.Models;
using Library.Services.DTOs;
using Mapster;

namespace Library.Services.Mappings
{
    public static class MappingsConfig
    {
        public static void ConfigureMappings()
        {
            TypeAdapterConfig<Patron, PatronResponseDto>.NewConfig();
            TypeAdapterConfig<PatronDto, Patron>.NewConfig();
            
            TypeAdapterConfig<Book, BookResponseDto>.NewConfig();

        }
    }
}
