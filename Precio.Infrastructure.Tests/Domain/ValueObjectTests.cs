using Microsoft.VisualStudio.TestTools.UnitTesting;
using Precio.Domain;

namespace Precio.DomainDrivenDesign.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ValueObjectTests
    {
        [TestMethod]
        public void ValueObjects_are_comparable_by_state()
        {
            var a1 = new Address
                         {
                             City = "Örebro",
                             Street = "Fabriksgatan",
                             ZipCode = "123"
                         };
            var a2 = new Address
                         {
                             City = "Örebro",
                             Street = "Fabriksgatan",
                             ZipCode = "123"
                         };
            var a3 = new Address
                         {
                             City = "aaa",
                             Street = "bbb",
                             ZipCode = "ccc"
                         };

            Assert.IsTrue(a1 == a2);
            Assert.IsTrue(a1 != a3);
        }

        [TestMethod]
        public void ValueObjects_can_be_copied()
        {
            var a1 = new Address
                         {
                             City = "Örebro",
                             Street = "Fabriksgatan",
                             ZipCode = "123"
                         };
            Address a2 = a1.Copy();

            Assert.IsTrue(a1 == a2);

            a1.City = "aaa";

            Assert.IsTrue(a1 != a2);
        }
    }

    public class Address : ValueObject<Address>
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}