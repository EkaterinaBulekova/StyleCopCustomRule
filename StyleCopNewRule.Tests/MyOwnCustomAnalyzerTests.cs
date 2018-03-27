using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCop;

namespace StyleCopNewRule.Tests
{
    [TestClass]
    public class MyOwnCustomAnalyzerTests
    {
        private const string TestControllers = @".\Controllers";
        private StyleCopConsole _scConsole;
        private List<Violation> _violations;


        [TestInitialize]
        public void Initialaze()
        {
            _scConsole = new StyleCopConsole(null, true, null, null, true, null);
            _violations = new List<Violation>();
            _scConsole.ViolationEncountered += scConsole_ViolationEncontered;
            _scConsole.OutputGenerated += scConsole_OutputGenerated;
        }

        [TestCleanup]
        public void Destroy()
        {
            _scConsole.ViolationEncountered -= scConsole_ViolationEncontered;
            _scConsole.OutputGenerated -= scConsole_OutputGenerated;
            _scConsole = null;
        }

        private void scConsole_OutputGenerated(object sender, OutputEventArgs e)
        {
            Console.WriteLine(e.Output);
        }

        private void scConsole_ViolationEncontered(object sender, ViolationEventArgs e)
        {
            _violations.Add(e.Violation);
        }



        [TestMethod]
        public void RightControllerNameAndAtribbute()
        {
            var project = new CodeProject(1, TestControllers, new Configuration(new string[] { "DEBUG" }));
            _scConsole.Core.Environment.AddSourceCode(project, TestControllers+ @"\RightController.cs", null);
            _scConsole.Start(new List<CodeProject>() { project }, true);

            Assert.AreEqual(0, _violations.Count);
        }

        [TestMethod]
        public void WrongControllerName()
        {
            var project = new CodeProject(1, TestControllers, new Configuration(new string[] { }));
            _scConsole.Core.Environment.AddSourceCode(project, TestControllers + @"\ControllerWrongName.cs", null);
            _scConsole.Start(new List<CodeProject>() { project }, true);

            Assert.AreEqual(1, _violations.Count);
        }

        [TestMethod]
        public void WrongControllerAtribute()
        {
            var project = new CodeProject(1, TestControllers, new Configuration(new string[] { "DEBUG" }));
            _scConsole.Core.Environment.AddSourceCode(project, TestControllers + @"\WrongAttributeController.cs", null);
            _scConsole.Start(new List<CodeProject>() { project }, true);

            Assert.AreEqual(1, _violations.Count);
        }

        [TestMethod]
        public void ControllerWithoutAtribute()
        {
            var project = new CodeProject(1, TestControllers, new Configuration(new string[] { "DEBUG" }));
            _scConsole.Core.Environment.AddSourceCode(project, TestControllers + @"\WithoutAttributeController.cs", null);
            _scConsole.Start(new List<CodeProject>() { project }, true);

            Assert.AreEqual(1, _violations.Count);
        }

        [TestMethod]
        public void ControllerWithoutAtributeWrongName()
        {
            var project = new CodeProject(1, TestControllers, new Configuration(new string[] { "DEBUG" }));
            _scConsole.Core.Environment.AddSourceCode(project, TestControllers + @"\ControllerWrongNameAttributer.cs", null);
            _scConsole.Start(new List<CodeProject>() { project }, true);

            Assert.AreEqual(2, _violations.Count);
        }
    }
}
