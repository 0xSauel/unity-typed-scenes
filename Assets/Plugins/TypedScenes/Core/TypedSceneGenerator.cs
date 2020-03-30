﻿using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace IJunior.TypedScenes
{
    public class TypedSceneGenerator
    {
        public static string Generate(string className, string GUID)
        {
            var targetUnit = new CodeCompileUnit();
            var targetNamespace = new CodeNamespace(TypedSceneSettings.Namespace);
            var targetClass = new CodeTypeDeclaration(className);
            targetClass.BaseTypes.Add("TypedScene");

            var pathConstant = new CodeMemberField(typeof(string), "GUID");
            pathConstant.Attributes = MemberAttributes.Private | MemberAttributes.Const;
            pathConstant.InitExpression = new CodePrimitiveExpression(GUID);
            targetClass.Members.Add(pathConstant);

            var loadMethod = new CodeMemberMethod();
            loadMethod.Name = "Load";
            loadMethod.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            loadMethod.Statements.Add(new CodeSnippetExpression("LoadScene(GUID)"));
            targetClass.Members.Add(loadMethod);

            targetNamespace.Types.Add(targetClass);
            targetUnit.Namespaces.Add(targetNamespace);

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions();
            options.BracingStyle = "C";

            var code = new StringWriter();
            provider.GenerateCodeFromCompileUnit(targetUnit, code, options);

            return code.ToString();
        }
    }
}
