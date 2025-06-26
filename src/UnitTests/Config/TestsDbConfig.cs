using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Config
{
    public static class TestsDbConfig
    {
        public static DefaultDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DefaultDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new DefaultDbContext(options);
        }
    }
}
