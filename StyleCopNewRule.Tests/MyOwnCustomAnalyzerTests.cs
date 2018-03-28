using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCop;

namespace StyleCopNewRule.Tests
{
    [TestClass]
    public class MyOwnCustomAnalyzerTests
    {
        private const string TestProjectPath = @"..\..\";
        private const string TestControllersPath = TestProjectPath + @"\Controllers";
        private const string TestEntitiesPath = TestProjectPath + @"\Entities";
        private CodeProject _project;
        private StyleCopConsole _scConsole;
        private List<Violation> _violations;

        [TestInitialize]
        public void Initialaze()
        {
            _scConsole = new StyleCopConsole(TestProjectPath, true, null, null, true, null);
            _project = new CodeProject(1, TestProjectPath, new Configuration(new string[0]));
            _violations = new List<Violation>();
            _scConsole.ViolationEncountered += scConsole_ViolationEncontered;
            _scConsole.OutputGenerated += scConsole_OutputGenerated;
        }

        [TestCleanup]
        public void Destroy()
        {
            _project = null;
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
        public void RightEntityTest()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestEntitiesPath + @"\RightEntity.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(0, _violations.Count);
        }

        [TestMethod]
        public void NonPublicEntityTest()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestEntitiesPath + @"\NonPublicEntity.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.AreEqual("HE2223", _violations[0].Rule.CheckId);
        }

        [TestMethod]
        public void EntityWithoutAttributeTest()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestEntitiesPath + @"\EntityWithoutAttribute.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.AreEqual("HE2225", _violations[0].Rule.CheckId);
        }

        [TestMethod]
        public void EntityWrongAttributeTest()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestEntitiesPath + @"\EntityWrongAttribute.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.AreEqual("HE2225", _violations[0].Rule.CheckId);
        }

        [TestMethod]
        public void EntityWithoutNameTest()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestEntitiesPath + @"\EntityWithoutName.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.AreEqual("HE2226", _violations[0].Rule.CheckId);
        }

        [TestMethod]
        public void EntityNonPublicNameTest()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestEntitiesPath + @"\EntityNonPublicName.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.AreEqual("HE2226", _violations[0].Rule.CheckId);
        }

        [TestMethod]
        public void EntityWithoutIdTest()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestEntitiesPath + @"\EntityWithoutId.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.AreEqual("HE2224", _violations[0].Rule.CheckId);
        }

        [TestMethod]
        public void EntityNonPublicIdTest()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestEntitiesPath + @"\EntityNonPublicId.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.AreEqual("HE2224", _violations[0].Rule.CheckId);
        }

        [TestMethod]
        public void RightControllerNameAndAtribbute()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestControllersPath + @"\RightController.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(0, _violations.Count);
        }

        [TestMethod]
        public void WrongControllerName()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestControllersPath + @"\ControllerWrongName.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.AreEqual("HE2227", _violations[0].Rule.CheckId);
        }

        [TestMethod]
        public void WrongControllerAtribute()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestControllersPath + @"\WrongAttributeController.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.AreEqual("HE2228", _violations[0].Rule.CheckId);
        }

        [TestMethod]
        public void ControllerWithoutAtribute()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestControllersPath + @"\WithoutAttributeController.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.AreEqual("HE2228", _violations[0].Rule.CheckId);
        }

        [TestMethod]
        public void ControllerWithoutAtributeWrongName()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestControllersPath + @"\ControllerWrongNameAttribute.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(2, _violations.Count);
            Assert.IsTrue(_violations.Any(_ => _.Rule.CheckId == "HE2227"));
            Assert.IsTrue(_violations.Any(_ => _.Rule.CheckId == "HE2228"));
        }

        [TestMethod]
        public void ControllerWithoutAtributeMethod()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestControllersPath + @"\WithoutAttributeMetodController.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.IsTrue(_violations.Any(_ => _.Rule.CheckId == "HE2228"));
        }
        
        [TestMethod]
        public void ControllerWrongAtributeMethod()
        {
            _scConsole.Core.Environment.AddSourceCode(_project, TestControllersPath + @"\WrongAttributeMetodController.cs", null);
            _scConsole.Start(new List<CodeProject> { _project }, true);

            Assert.AreEqual(1, _violations.Count);
            Assert.IsTrue(_violations.Any(_ => _.Rule.CheckId == "HE2228"));
        }
    }
}
