using Microsoft.EntityFrameworkCore;

namespace Event_Registration.Tests.Setup
{
    public class TestFixture : IDisposable
    {
        private string _databaseName;

        public TestFixture()
        {
            _databaseName = $"TestDatabase_{Guid.NewGuid()}";
        }

        public ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: _databaseName)
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        public void Dispose()
        {

        }
    }
}
