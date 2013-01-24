using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.IO;
using System.CodeDom;
using System.Diagnostics;


namespace ProUpgradeEditor.Common
{

    public class CodeCompiler : IDisposable
    {
        bool disposed;

        public CodeCompiler()
        {
            disposed = false;

            CodeProvider = CodeDomProvider.CreateProvider("CSharp", new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });
            CompilerParameters = new CompilerParameters();

            CompilerParameters.GenerateExecutable = false;
            CompilerParameters.GenerateInMemory = true;
            CompilerParameters.IncludeDebugInformation = true;
            CompilerParameters.TempFiles = new TempFileCollection(Environment.GetEnvironmentVariable("TEMP"), true);
            CompilerParameters.TreatWarningsAsErrors = false;

            var references = Assembly.GetAssembly(typeof(CodeCompiler)).GetReferencedAssemblies();
            CompilerParameters.ReferencedAssemblies.AddRange(references.Select(x => Assembly.Load(x.FullName).Location).ToArray());
            CompilerParameters.ReferencedAssemblies.Add(Assembly.GetAssembly(GetType()).Location);
        }

        protected CodeDomProvider CodeProvider;
        protected CompilerParameters CompilerParameters;

        public Assembly Compile(string csharpText)
        {
            return Compile(csharpText.MakeEnumerable<string>());
        }

        public Assembly Compile(IEnumerable<string> csharpText)
        {
            List<CompilerError> errors;
            var ret = Compile(csharpText, out errors);
            if (ret == null)
            {
                errors.ForEach(x => x.ToString().OutputDebug());
            }
            return ret;
        }
        public Assembly Compile(IEnumerable<string> csharpText, out List<CompilerError> errors)
        {
            errors = new List<CompilerError>();

            var result = CodeProvider.CompileAssemblyFromSource(CompilerParameters, csharpText.ToArray());

            if (result != null && result.Errors != null && result.Errors.Count > 0)
            {
                errors.AddRange(result.Errors.ToEnumerable<CompilerError>());
                errors.ForEach(x => x.ToString().OutputDebug());
            }

            return result.CompiledAssembly;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                try
                {
                    if (CodeProvider != null)
                    {
                        CodeProvider.Dispose();
                    }
                }
                finally { CodeProvider = null; }
                CompilerParameters = null;
                disposed = true;
            }
        }
    }
}
