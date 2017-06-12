using System;
using System.Diagnostics.Eventing.Reader;
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



        }

        [TestMethod]
        public void GetUserIdByFullName()
        {
            var user = _userBeheerRepository.GetUser("TestUser");
            int? id = _userBeheerRepository.GetUserIdByFullName("Test User");
            Assert.AreEqual(user.Id,id);


        }

        [TestMethod]
        public void DosUserExist()
        {
            var user = _userBeheerRepository.GetUser("TestUser");
            Assert.IsTrue(_userBeheerRepository.DoesUserExist(user.Username));

            _userBeheerRepository.RemoveUser(user.Id);
        }

        [TestMethod]
        public void GetAllUsers()
        {
            var user = _userBeheerRepository.GetUser("Driver");
            bool found = false;

            foreach (User item in _userBeheerRepository.GetAllUsers())
            {

                if (JsonConvert.SerializeObject(user) == JsonConvert.SerializeObject(item))
                {
                    found = true;
                }

            }
            
            Assert.IsTrue(found);
        }

    }
}
