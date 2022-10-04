using Autofac;
using Business.Abstract;
using Business.Concrete;
using Core.Extension;
using NUnit.Framework;

namespace UnitTest
{
    public class UserServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test(Author = "Can Cil", 
              TestOf = typeof(UserService),
              Description = "To test service of UserService")]
        public void Test1()
        {
            try
            {
                var builder = new ContainerBuilder();
                var container = builder.RegisterServicesToTest();
                container.Resolve<IUserService>();
                container.Dispose();
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            
        }
    }
}
