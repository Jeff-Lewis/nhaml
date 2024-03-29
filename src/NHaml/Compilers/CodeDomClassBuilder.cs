using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NHaml.Compilers
{
    public abstract class CodeDomClassBuilder : TemplateClassBuilder
    {
        public CodeMemberMethod RenderMethod{ get; set; }

        public CodeDomProvider CodeDomProvider { get; set; }

        public List<CodeTypeMember> Members { get; set; }

        protected int PreambleCount{ get; set; }

    	public CodeDomClassBuilder(string className)
            : base(className)
        {
            Members = new List<CodeTypeMember>();
            RenderMethod = new CodeMemberMethod
                               {
                                   Name = "CoreRender",
                                   Attributes = MemberAttributes.Override | MemberAttributes.Family,

                               };
            RenderMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(TextWriter), "textWriter"));
        }
        public override void AppendSilentCode(string code, bool closeStatement)
        {
			if (code != null)
			{
				code = code.Trim();

				if (!closeStatement)
				{
					code = code + Comment;
				}
				RenderMethod.Statements.Add(new CodeSnippetExpression
				                            {
				                            	Value = code,
				                            });

			}

        }

    	protected abstract string Comment { get; }

    	public override void AppendOutput(string value)
        {
            if (value == null)
            {
                return;
            }


            if (Depth > 0)
            {
                if (value.StartsWith(string.Empty.PadLeft(Depth*2), StringComparison.Ordinal))
                {
                    value = value.Remove(0, Depth*2);
                }
            }


            var writeInvoke = new CodeMethodInvokeExpression
                                  {
                                      Method = new CodeMethodReferenceExpression
                                                   {
                                                       MethodName = "Write",
                                                       TargetObject =
                                                           new CodeVariableReferenceExpression
                                                               {
                                                                   VariableName = CurrentTextWriterVariableName
                                                               }
                                                   }
                                  };
            writeInvoke.Parameters.Add(new CodePrimitiveExpression
                                           {
                                               Value = value
                                           });


            RenderMethod.Statements.Add(new CodeExpressionStatement
                                            {
                                                Expression = writeInvoke
                                            });

        }

        public override void AppendOutputLine()
        {
            var writeInvoke = new CodeMethodInvokeExpression
                                  {
                                      Method = new CodeMethodReferenceExpression
                                                   {
                                                       MethodName = "WriteLine",
                                                       TargetObject =
                                                           new CodeVariableReferenceExpression
                                                               {
                                                                   VariableName = CurrentTextWriterVariableName
                                                               }
                                                   }
                                  };
            RenderMethod.Statements.Add(new CodeExpressionStatement
                                            {
                                                Expression = writeInvoke
                                            });

        }

        public override void AppendCode(string code, bool escapeHtml)
        {
            if (code != null)
            {

                var write = new CodeMethodInvokeExpression
                                    {
                                        Method = new CodeMethodReferenceExpression
                                                     {
                                                         MethodName = "Write",
                                                         TargetObject =
                                                             new CodeVariableReferenceExpression
                                                                 {
                                                                     VariableName = CurrentTextWriterVariableName
                                                                 }
                                                     }
                                    };
                var toStringInvoke = new CodeMethodInvokeExpression
                                         {
                                             Method = new CodeMethodReferenceExpression
                                                          {
                                                              MethodName = "ToString",
                                                              TargetObject =
                                                                  new CodeVariableReferenceExpression
                                                                      {
                                                                          VariableName = "Convert"
                                                                      }
                                                          }
                                         };
                toStringInvoke.Parameters.Add(new CodeSnippetExpression
                                                  {
                                                      Value = code
                                                  });




                if (escapeHtml)
                {
                    var htmlEncodeInvoke = new CodeMethodInvokeExpression
                                               {
                                                   Method = new CodeMethodReferenceExpression
                                                                {
                                                                    MethodName = "HtmlEncode",
                                                                    TargetObject =
                                                                        new CodeVariableReferenceExpression { VariableName = "HttpUtility" }
                                                                }
                                               };
                    htmlEncodeInvoke.Parameters.Add(toStringInvoke);
                    write.Parameters.Add(htmlEncodeInvoke);
                }
                else
                {
                    write.Parameters.Add(toStringInvoke);
                }


                RenderMethod.Statements.Add(new CodeExpressionStatement
                                                {
                                                    Expression = write
                                                });
            }
        }

        public override void AppendChangeOutputDepth(int depth)
        {
            if (BlockDepth != depth)
            {

                var depthStatement = new CodeAssignStatement
                                         {
                                             Left = new CodePropertyReferenceExpression
                                                        {
                                                            PropertyName = "Depth",
                                                            TargetObject =
                                                                new CodeVariableReferenceExpression
                                                                    {
                                                                        VariableName = "Output"
                                                                    }
                                                        },
                                             Right = new CodePrimitiveExpression
                                                         {
                                                             Value = depth
                                                         }
                                         };

                RenderMethod.Statements.Add(depthStatement);

                BlockDepth = depth;
            }
      
        }

  


        public override void AppendAttributeTokens(string schema, string name, IList<ExpressionStringToken> values)
        {

            var _invoke1 = new CodeMethodInvokeExpression();
            _invoke1.Parameters.Add(new CodeVariableReferenceExpression
                                        {
                                            VariableName = CurrentTextWriterVariableName
                                        });

            _invoke1.Parameters.Add(new CodePrimitiveExpression
                                        {
                                            Value = schema
                                        });

            _invoke1.Parameters.Add(new CodePrimitiveExpression
                                        {
                                            Value = name
                                        });

            if (values.Count == 1)
            {
                var expressionStringToken = values[0];
                if (expressionStringToken.IsExpression)
                {
                    _invoke1.Parameters.Add(new CodeSnippetExpression
                                                {
                                                    Value = expressionStringToken.Value
                                                });
                }
                else
                {

                    _invoke1.Parameters.Add(new CodePrimitiveExpression
                                                {
                                                    Value = expressionStringToken.Value
                                                });
                }
            }
            else
            {
                var concatExpression = GetConcatExpression(values);
                _invoke1.Parameters.Add(concatExpression);
            }





            _invoke1.Method = new CodeMethodReferenceExpression
                                  {
                                      MethodName = "RenderAttributeIfValueNotNull",
                                      TargetObject = new CodeThisReferenceExpression()
                                  };
            RenderMethod.Statements.Add(new CodeExpressionStatement { Expression = _invoke1 });

        }

        public static CodeMethodInvokeExpression GetConcatExpression(IList<ExpressionStringToken> values)
        {
            var stringType = new CodeTypeReference(typeof(string));
            var arrayExpression = new CodeArrayCreateExpression
            {
                CreateType = new CodeTypeReference("System.String", 1)
                {
                    ArrayElementType = stringType
                },
                Size = 0
            };


            foreach (var expressionStringToken in values)
            {
                if (expressionStringToken.IsExpression)
                {
                    var toStringExpression = new CodeMethodInvokeExpression
                    {
                        Method = new CodeMethodReferenceExpression
                        {
                            MethodName = "ToString",
                            TargetObject =
                                new CodeVariableReferenceExpression
                                {
                                    VariableName = "Convert"
                                }
                        }
                    };

                    toStringExpression.Parameters.Add(new CodeSnippetExpression
                    {
                        Value = expressionStringToken.Value
                    });
                    arrayExpression.Initializers.Add(toStringExpression);
                }
                else
                {
                    var _value3 = new CodePrimitiveExpression
                    {
                        Value = expressionStringToken.Value
                    };
                    arrayExpression.Initializers.Add(_value3);
                }
            }
            var concatExpression = new CodeMethodInvokeExpression
            {
                Method = new CodeMethodReferenceExpression
                {
                    MethodName = "Concat",
                    TargetObject = new CodeTypeReferenceExpression
                    {
                        Type = stringType
                    }
                }
            };
            concatExpression.Parameters.Add(arrayExpression);
            return concatExpression;
        }
        public override void BeginCodeBlock()
        {
            Depth++;

            RenderBeginBlock();
        }

        public override void EndCodeBlock()
        {
            RenderEndBlock();
            Depth--;
        }

        protected abstract void RenderEndBlock();
        protected abstract void RenderBeginBlock();

        public override void AppendPreambleCode(string code)
        {

            var expression = new CodeSnippetExpression
                                 {
                                     Value = code,
                                 };

            //  RenderMethod.Statements.Insert(preambleCount, new CodeSnippetStatement(code));
            RenderMethod.Statements.Insert(PreambleCount, new CodeExpressionStatement(expression));
            PreambleCount++;
        }

        public override string Build(IList<string> imports)
        {
            
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {

                var compileUnit = new CodeCompileUnit();

                // Declares a namespace named TestNamespace.
                var testNamespace = new CodeNamespace();
                //testNamespace.Name = "TempNHamlNamespace";
                // Adds the namespace to the namespace collection of the compile unit.
                compileUnit.Namespaces.Add(testNamespace);

                foreach (var import in imports)
                {
                    var namespaceImport = new CodeNamespaceImport(import);
                    testNamespace.Imports.Add(namespaceImport);
    
                }
                

                var generator = CodeDomProvider.CreateGenerator(writer);
                var options = new CodeGeneratorOptions();
                var declaration = new CodeTypeDeclaration
                                      {
                                          Name = ClassName, IsClass = true
                                      };
               
                declaration.BaseTypes.Add(BaseType);
                declaration.Members.Add(RenderMethod);
                declaration.Members.AddRange(Members.ToArray());


//                var codeNamespace = new CodeNamespace();
                testNamespace.Types.Add(declaration);
                generator.GenerateCodeFromNamespace(testNamespace, writer, options);

                //TODO: implement IDisposable
                writer.Close();
            }

            return builder.ToString();
           
        }
    }
}