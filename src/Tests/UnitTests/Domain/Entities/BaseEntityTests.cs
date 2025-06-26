using Domain.Entities;

namespace UnitTests.Domain.Entities
{
    public class BaseEntityTests
    {
        [Fact]
        public void BaseEntity_DefaultValues_AreSet()
        {
            var entity = new BaseEntity();

            Assert.NotEqual(Guid.Empty, entity.Id);
            Assert.True(entity.CreatedAt <= DateTime.UtcNow);
            Assert.Equal(Guid.Empty, entity.CreatedBy);
            Assert.Null(entity.UpdatedAt);
            Assert.Null(entity.DeletedAt);
            Assert.False(entity.IsDeleted ?? false);
        }
    }
}
