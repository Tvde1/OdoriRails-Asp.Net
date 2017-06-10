using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OdoriRails.Helpers.DAL.Repository;
using OdoriRails.Helpers.Objects;

namespace OdoriRails.Tests
{
    [TestClass]
    public class UserTests
    {
        readonly UserBeheerRepository _userBeheerRepository = new UserBeheerRepository();

        [TestMethod]
        public void AddUser()
        {
            var checkUser = _userBeheerRepository.GetUser("TestUser");
            if (checkUser != null) DeleteUser();

            var user = new User(0, "Test User", "TestUser", "testuser@remise.nl", "test", Role.Administrator, null);
            user = _userBeheerRepository.AddUser(user);

            var fetchUser = _userBeheerRepository.GetUser("TestUser");

            Assert.AreEqual(JsonConvert.SerializeObject(user), JsonConvert.SerializeObject(fetchUser));
        }

        [TestMethod]
        public void DeleteUser()
        {
            var user = _userBeheerRepository.GetUser("TestUser");

            if (user == null)
            {
                Assert.IsTrue(true);
                return;
            }

            _userBeheerRepository.RemoveUser(user.Id);

            var fetchUser = _userBeheerRepository.GetUser("TestUser");

            Assert.IsNull(fetchUser);
        }
    }
}
