using Autossential.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Collections.Generic;
using System.Linq;

namespace Autossential.Tests
{
    [TestClass]
    public class RandomStringTests
    {
        [TestMethod]
        public void GenerateString_WithDefaultFormat_ReturnsCorrectLength()
        {
            var result = WorkflowInvoker.Invoke(new RandomString());
            Assert.AreEqual(7, result.Length);  // "Aa*0*Aa" default format
        }

        [TestMethod]
        public void GenerateString_WithCustomFormat_ReturnsCorrectString()
        {
            var result = WorkflowInvoker.Invoke(new RandomString(), new Dictionary<string, object>
            {
                ["Format"] = "Aa0"
            });

            Assert.AreEqual(3, result.Length);  // "Aa0" three chars lenght
            Assert.IsTrue(char.IsUpper(result[0]));  // First must be upper case
            Assert.IsTrue(char.IsLower(result[1]));  // Second must be lower case
            Assert.IsTrue(char.IsDigit(result[2]));  // Third must be digit
        }

        [TestMethod]
        public void GenerateString_WithCustomCharacters_IncludesCustomCharacter()
        {
            var result = WorkflowInvoker.Invoke(new RandomString(), new Dictionary<string, object>
            {
                ["Format"] = "?*",
                ["Custom"] = "@#_"
            });

            Assert.AreEqual(2, result.Length);  // "?" e "*" deve gerar uma string de 2 caracteres.
            Assert.IsTrue("@#_".Contains(result[0]));  // O primeiro caractere deve ser um dos caracteres customizados.
        }

        [TestMethod]
        public void GenerateString_WithEscapeCharacters_ReturnsEscapedString()
        {
            var result = WorkflowInvoker.Invoke(new RandomString(), new Dictionary<string, object>
            {
                ["Format"] = @"A\*a"
            });

            Assert.AreEqual(3, result.Length);  // "A\*a" deve gerar uma string de 3 caracteres.
            Assert.IsTrue(char.IsUpper(result[0]));  // O primeiro caractere deve ser maiúsculo.
            Assert.AreEqual('*', result[1]);  // O segundo caractere deve ser um asterisco literal.
            Assert.IsTrue(char.IsLower(result[2]));  // O terceiro caractere deve ser minúsculo.
        }
    }
}
