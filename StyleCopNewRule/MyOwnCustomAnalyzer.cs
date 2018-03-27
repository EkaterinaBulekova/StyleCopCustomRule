using System.Linq;

namespace StyleCopNewRule
{
    using StyleCop;
    using StyleCop.CSharp;
    using System;

    [SourceAnalyzer(typeof(CsParser))]
    public class MyOwnCustomAnalyzer : SourceAnalyzer
    {
        public override void AnalyzeDocument(CodeDocument currentCodeDocument)
        {
            var codeDocument = (CsDocument)currentCodeDocument;
            if (codeDocument.RootElement != null && !codeDocument.RootElement.Generated)
            {
                codeDocument.WalkDocument(new CodeWalkerElementVisitor<object>(this.InspectCurrentElement), null, null);
            }
        }

        private bool InspectCurrentElement(CsElement element, CsElement parentElement, object context)
        {
            if (element.ElementType == ElementType.Namespace && element.Declaration.Name.EndsWith(".Entities"))
            {
                var classElements = element.ChildElements.Where(_ => _.ElementType == ElementType.Class);

                foreach (var clElement in classElements)
                {
                    if (clElement is Class cl)
                    {
                        if(cl.AccessModifier != AccessModifierType.Public)
                            AddViolation(parentElement, "ClassUnderEntitiesNamespaceMustBePublic", "ClassUnderEntitiesNamespaceMustBePublic");

                        if (!clElement.Attributes.Any(_ => _.ChildTokens.Any(t => t.Text.Contains("DataContract"))))
                            AddViolation(parentElement, "ClassUnderEntitiesNamespaceMustHaveAttribute", "ClassUnderEntitiesNamespaceMustHaveAttribute");
                    }

                    var propElements = clElement.ChildElements.Where(_ => _.ElementType == ElementType.Property).ToList();

                    var idElement = propElements.FirstOrDefault(_ => _.Declaration.Name.Equals("Id", StringComparison.Ordinal));
                    if(idElement == null || idElement.AccessModifier != AccessModifierType.Public)
                        AddViolation(parentElement, "ClassUnderEntitiesNamespaceMustHavePublicId", "ClassUnderEntitiesNamespaceMustHavePublicId");

                    var nameElement = propElements.FirstOrDefault(_ => _.Declaration.Name.Equals("Name", StringComparison.Ordinal));
                    if (nameElement == null || nameElement.AccessModifier != AccessModifierType.Public)
                        AddViolation(parentElement, "ClassUnderEntitiesNamespaceMustHavePublicName", "ClassUnderEntitiesNamespaceMustHavePublicName");
                }
            }

            if (element is Class cClass && cClass.BaseClass.Contains("Controller"))
            {
                var hasMvcUsingParent = parentElement?.ChildElements
                    .Any(_ => _.ElementType == ElementType.UsingDirective && _.Declaration.Name.Equals("System.Web.Mvc", StringComparison.Ordinal)) ?? false;

                var hasMvcUsingRoot = parentElement.FindParentElement()?.ChildElements
                    .Any(_ => _.ElementType== ElementType.UsingDirective && _.Declaration.Name.Equals("System.Web.Mvc", StringComparison.Ordinal)) ?? false;

                if (hasMvcUsingParent || hasMvcUsingRoot  )
                {
                    if (!cClass.Name.EndsWith("Controller"))
                        AddViolation(element, "SystemWebMvcControllerMustHaveSuffixController", "SystemWebMvcControllerMustHaveSuffixController");

                    if (!element.Attributes.Any(_ => _.ChildTokens.Any(t => t.Text.Contains("Authorize"))))
                    {
                        var publicMethods = element.ChildElements.Where(_ =>
                            _.ElementType == ElementType.Method && _.AccessModifier == AccessModifierType.Public).ToList();
                            
                        var hasNonAutorizePublicMethods = !publicMethods.Any() || publicMethods.Any(_=>!_.Attributes.Any(a => a.ChildTokens.Any(t => t.Text.Contains("Authorize"))));

                        if (hasNonAutorizePublicMethods)
                            AddViolation(element, "SystemWebMvcControllerMustHaveAttribute", "SystemWebMvcControllerMustHaveAttribute");


                    }
                }
            }

            return true;
        }
    }
}

