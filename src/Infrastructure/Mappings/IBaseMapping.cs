using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Mappings
{
    public interface IBaseMapping
    {
        void MapEntity(ModelBuilder modelBuilder);
    }
}
